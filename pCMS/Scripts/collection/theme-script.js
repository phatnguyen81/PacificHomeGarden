jQuery(document).ready(function() {
	jQuery(window).resize(
		function(){
			if(!jQuery('body').hasClass('cherry-fixed-layout')) {
				jQuery('.full-width-block').width(jQuery(window).width());
				jQuery('.full-width-block').css({width: jQuery(window).width(), "margin-left": (jQuery(window).width()/-2), left: "50%"});
			};
		}
	).trigger('resize');

	jQuery(".sf-menu > li > a").each(function(){
		var btn_text = jQuery(this).text();
		jQuery(this).empty().append('<span data-hover="'+btn_text+'">'+btn_text+'</span>');
	});

	jQuery('.page-template-page-home div.type-page > .row').each(function(){
		if( !jQuery(this).hasClass('without-background') ) {
			jQuery(this).children('[class*="span"]').not('.span12').wrapAll('<div class="span12"><div class="row"></div></div>');
		}
	});

	jQuery('#content.span8').each(function(){
		if( jQuery(this).hasClass('left') ) {
			jQuery('#sidebar').addClass('left');
		}
	});
})