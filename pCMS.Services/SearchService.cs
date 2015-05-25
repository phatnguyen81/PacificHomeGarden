using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using pCMS.Core;
using pCMS.Core.Domain;

namespace pCMS.Services
{
    
    public interface ISearchService
    {
        //void AddContent(Guid id, DocumentType type, string title, string content, string keywords);
        void AddContent(DocumentSearchItem document);
        //void UpdateContent(Guid id, DocumentType type, string title, string content, string keywords);
        void UpdateContent(DocumentSearchItem document);
        void DeleteContent(Guid id);
        IPagedList<DocumentSearchItem> QueryContent(string keywords, int pageindex, int pagesize);
    }

    public class SearchService : ISearchService, IDisposable
    {
        private readonly string _indexFileLocation = HttpContext.Current.Server.MapPath("~/index");
        private IndexWriter _indexWriter;

        private Document CreateDoc(DocumentSearchItem document)
        {
            var doc = new Document();

            doc.Add(new Field(
                        "ID",
                        document.Id.ToString(), Field.Store.YES,
                        Field.Index.NOT_ANALYZED));
            doc.Add(new Field(
                        "PARENTID",
                        document.ParentId.ToString(), Field.Store.YES,
                        Field.Index.NOT_ANALYZED));
            doc.Add(new Field(
                        "TYPE",
                        ((int)document.Type).ToString(),
                        Field.Store.YES,
                        Field.Index.NOT_ANALYZED));

            doc.Add(new Field(
                        "TITLE",
                        document.Title ?? string.Empty,
                        Field.Store.YES,
                        Field.Index.ANALYZED));

            doc.Add(new Field(
                        "CONTENT",
                        document.Content == null ?string.Empty : Regex.Replace(document.Content, "<(.|\n)*?>", "") ,
                        Field.Store.YES,
                        Field.Index.ANALYZED));
            doc.Add(new Field(
                        "KEYWORDS",
                        document.Keywords ?? string.Empty,
                        Field.Store.YES,
                        Field.Index.ANALYZED));
            return doc;
        }

        private IndexWriter CurrentIndexWriter
        {
            get
            {
                if(_indexWriter != null) return _indexWriter;
                var dirInfo = new DirectoryInfo(_indexFileLocation);
                Lucene.Net.Store.Directory dir = FSDirectory.Open(dirInfo);

                Lucene.Net.Analysis.Analyzer analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);

                _indexWriter = new IndexWriter(dir, analyzer, !IndexReader.IndexExists(FSDirectory.Open(dirInfo)), IndexWriter.MaxFieldLength.UNLIMITED);
                return _indexWriter;
            }
        }
        public SearchService()
        {
            _indexWriter = null;
        }

        public void Dispose()
        {
            if(_indexWriter != null)
            _indexWriter.Dispose();
        }

        //public void AddContent(Guid id, DocumentType type, string title, string content, string keywords)
        //{
        //    var doc = new DocumentSearchItem
        //                  {
        //                      Id = id,
        //                      Type = type,
        //                      Title = title,
        //                      Content = content,
        //                      Keywords = keywords,
        //                      ParentId = 
        //                  };
        //    AddContent(doc);
        //}

        public void AddContent(DocumentSearchItem document)
        {
            CurrentIndexWriter.AddDocument(CreateDoc(document));
        }

        //public void UpdateContent(Guid id, DocumentType type, string title, string content, string keywords)
        //{
        //    var doc = new DocumentSearchItem
        //    {
        //        Id = id,
        //        Type = type,
        //        Title = title,
        //        Content = content,
        //        Keywords = keywords
        //    };
        //    UpdateContent(doc);
        //}

        public void UpdateContent(DocumentSearchItem document)
        {
            CurrentIndexWriter.UpdateDocument(new Term("ID", document.Id.ToString()), CreateDoc(document));
        }

        public void DeleteContent(Guid id)
        {
            CurrentIndexWriter.DeleteDocuments(new Term("ID", id.ToString()));
        }

        public IPagedList<DocumentSearchItem> QueryContent(string keywords, int pageindex, int pagesize)
        {

            Lucene.Net.Analysis.Analyzer analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
            var parser = new Lucene.Net.QueryParsers.MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29,  new[]{"TITLE","CONTENT"}, analyzer );
            Lucene.Net.Search.Query query = null;

            try
            {
                if (string.IsNullOrEmpty(keywords))
                {
                    throw new Exception("You forgot to enter something to search for...");
                }
    
                query = parser.Parse(keywords);
    
            }
            catch (Exception)
            {
                
            }
            var dirInfo = new DirectoryInfo(_indexFileLocation);
            var searcher = new IndexSearcher(FSDirectory.Open(dirInfo), true);

            var collector = TopScoreDocCollector.Create(100, true);
            searcher.Search(query, collector);
            var results = new List<DocumentSearchItem>();
            var hits = collector.TopDocs().ScoreDocs;
            var offset = pageindex * pagesize;
            var count = Math.Min(hits.Length - offset, pagesize);
            foreach (var scoreDoc in hits)
            {
                var docId = scoreDoc.Doc;
                var doc = searcher.Doc(docId);
                results.Add(new DocumentSearchItem
                                {
                                    Id = new Guid(doc.Get("ID")),
                                    ParentId = new Guid(doc.Get("PARENTID")),
                                    
                                    Type = (DocumentType)int.Parse(doc.Get("TYPE")),
                                    Title = doc.Get("TITLE"),
                                    Content = doc.Get("CONTENT"),
                                    Keywords = doc.Get("KEYWORDS")
                                });
            }
            var searchitems = new PagedList<DocumentSearchItem>(results, pageindex, pagesize);
            return searchitems;
        }
    }
}
