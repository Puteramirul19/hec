(function($){
  "use strict";
  $(function()
  {
    var hec = {
      mainHeight : function() {
        var winHeight = $(window).height();
        //var footerHeight = $('.footer').height();
        $('#main-content').css({
          'height' : winHeight
        });
        $('#myCarousel .carousel-inner .item').css({
          'height' : winHeight
        });
      },
      tooltip: function() {
       $('[data-toggle="tooltip"]').tooltip();

      },
      floatingHighlight: function() {
       $('#highlight-block').click(function() {
          $(this).addClass('active');
          if ($(this).hasClass('active')) {
            $(this).parent().animate({
              right: 0
            });
            $(this).removeClass('active');
            //$('#highlight-block-wrapper').css({
            //  'z-index': 10
            //});
          }
        });
      },
      closeFloatingHighlight: function() {
        $('.close-button').on('click', function() {
          $('#highlight-block-wrapper').animate({
              right: '-460px'
            });
          //$('#highlight-block-wrapper').css({
          //    'z-index': 0
          //  });
           if ($(window).width() <= 450) {
            $('#highlight-block-wrapper').animate({
                right: '-310px'
              });
          }
					$(".floating-tab2.tariff").removeClass("active");
					$(".floating-tab2.how").removeClass("active");
        });
      },
      resultDisplay: function() {
        $('.bill-calculator .calculate-all').click(function() {
          $('.bill-calculator .result-calculate-wrapper').show( "slow" );
         
        });

      },
      closePopupRoom: function() {
        $('.house-container .close-button').on('click', function() {
          window.history.back();
        });
      },
      showAll: function()
      {   
        $(".show-button-wrapper").click(function() {
        $(".showall").slideDown("slow");
        $(".show-button-wrapper").css("display" , "none");

        return false;
      });
      },
      deleteFriend : function() {
      var iconRemoveFriend = $('.friend-list .remove-friend');
      var friendRemove = $('.friend-list li');
      iconRemoveFriend.on('click', function(){
        $(this).parent().remove();
      });
      },
      deleteRowRank : function() {
      var iconDustbin = $('.friend-list-rank .icon-dustbin');
      var rowRankRemove = $('.friend-list-rank ul li');
      iconDustbin.on('click', function(){
        $(this).parent().remove();
      });
      },
	  tariffBlock : function() {
		$(".floating-tab2.tariff").click(function() {
					
					
			if ($(this).hasClass('active')){
					
				
				$(".tariff-content").css({
						"display" : "block",
						"z-index" : "10"
					});
					$(".how-content").css({
						"display" : "none",
						"z-index" : "0"
					});
			}
				else {
							
					$(".floating-tab2.tariff").addClass("active");
					$(".tariff-content").css({
						"display" : "block",
						"z-index" : "10"
					});
					$(".how-content").css({
						"display" : "none",
						"z-index" : "0"
					});
							
							
					$(this).parents("div#highlight-block-wrapper").animate({
						right: 0
					});
				}
					
					
		});

			$(".floating-tab2.how").click(function() {
					
					
			if ($(this).hasClass('active')){
					
				
				$(".tariff-content").css({
						"display" : "none",
						"z-index" : "0"
					});
					$(".how-content").css({
						"display" : "block",
						"z-index" : "10"
					});
			}
				else {
					$(".floating-tab2.how").addClass("active");
					$(".tariff-content").css({
						"display" : "none",
						"z-index" : "0"
					});
					$(".how-content").css({
						"display" : "block",
						"z-index" : "10"
					});
							
							
					$(this).parents("div#highlight-block-wrapper").animate({
						right: 0
					});
				}
					
					
		});
				
	},
	  displayButton: function () {
	      if ($(window).width() <= 450) {
          $(".add-button.top").css({
            "display" : "block"
          });
        }   
      },
      floatingAmount: function() {
        if ($(window).width() <= 450) {
          $(window).scroll(function() {
            if ($(this).scrollTop() > 0) {
                $('.persist-header .title-store').addClass('amount-fixed');
            } else if ($(this).scrollTop() < 5) {
                $('.persist-header .title-store').removeClass('amount-fixed');
            }
        });
        }
      }
    };
    
    //hec.mainHeight();
    hec.tooltip();
    hec.floatingHighlight();
    hec.closeFloatingHighlight();
    hec.resultDisplay();
    hec.closePopupRoom();
    hec.showAll();
    hec.deleteRowRank();
    hec.deleteFriend();
    hec.tariffBlock();
    hec.displayButton();
    hec.floatingAmount();
  });

  
//$(window).resize(function() {
// if ($(window).width()< 768){ 
//       window.location.href = window.location.href;
//    }  
//});     

})(jQuery);

