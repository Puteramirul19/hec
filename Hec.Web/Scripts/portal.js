(function($){
	"use strict";
	$(function()
	{
		var snm = {
			sliderAnnouncement : function(){
				$('.bxslider').bxSlider({
				  auto: true,
				  autoControls: true,
				  speed: 2000
				});
			},
			menuTab : function() {
				$(".tabs-menu a").click(function(event) {
			        event.preventDefault();
			        $(this).parent().addClass("current");
			        $(this).parent().siblings().removeClass("current");
			        var tab = $(this).attr("href");
			        $(".tab-content").not(tab).css("display", "none");
			        $(tab).show();

			        if($(this).parent().hasClass("manual")){
			        	if($(".subtree").val().length == 0 ){
							$("ul.manual-submenu").appendTo(".subtree");
							if($("ul.manual-submenu").css("display") == "none"){
								$("ul.manual-submenu").css({'display':'block'});
							}
							else {
								$("ul.manual-submenu").css({'display':'none'});
							}
						}
			        }
			        else {
			        	$("ul.manual-submenu").css({'display':'none'});
			        }
			    });
			},
			inputFocusBlur : function(){
				$('.labelinput')
				.on('focus', function(){
				      var $this = $(this);
				      if($this.val() != ''){
				          $this.val('');
				      }
				  })
				  .on('blur', function(){
				      var $this = $(this);
				      if($this.val() == ''){
				          $this.val($this.attr('title'));
				      }
				  });
			},
			datePicker : function(){
				$("#datepicker").datepicker({
			    format: 'dd-mm-yyyy'
				});
			}
		};
		
		snm.sliderAnnouncement();
		snm.inputFocusBlur();
		snm.menuTab();
		snm.datePicker();

	});
})(jQuery);