(function($){
  "use strict";
  $(function()
  {
    var hec1 = {
      customScrollBar : function() {
        $(".content-appliance-store").mCustomScrollbar({
      theme:"light-thick"
    });

    $(".content-appliance-store1").mCustomScrollbar({
      theme:"light-thick" ,
      callbacks:{
      whileScrolling:function(){
        if (this.mcs.draggerTop >= 10) {
           $('.persist-header').addClass('sticky-fixed');
            if ($(window).width() <= 450) {
               $('.persist-header .title-store').addClass('amount-fixed');
            }
        }
        else {
           $('.persist-header').removeClass('sticky-fixed');
           $('.persist-header .title-store').removeClass('amount-fixed');

            
        }
      },
      onUpdate:function(){
       var stickyHeader = $('.tabbing-content').width();
       $('.persist-header').css({
          'width': stickyHeader  
         });
      }
    }

    });
    
    
    var winHeight = $(window).height() - 80;
    var popupWidth = $('.container-popup').width();
    var winWidth = $(window).width() - popupWidth - 400;
    
    $('.mCustomScrollBox').css({
      'height': winHeight  
    });
    $('.mCSB_container').css({
          'margin-right': winWidth  
        });

    $("#highlight-block-content").mCustomScrollbar({
      theme:"light-thick"
    });
    
      },
    roomSelection : function() {
    var roomSelect = $('.house-container.room-select');
        roomSelect.find(".house").on('click', function(){
       
        if ($(this).hasClass('select')) {
            $(this).removeClass('select');
           
        } else {
            $(this).addClass('select');  
            $('.add-remove > span').on('click', function(e) {
            e.stopPropagation(); 
        });
        }
      });
       
      },

    applianceTab : function() {
    var tabSelect = $('.tab-select > ul > li');
    var contentSelect = $('.tab-content');
        tabSelect.on('click', function(){
      var pos = $(this).index();
      contentSelect.find('> div').hide().eq(pos).show();
      tabSelect.removeClass('liactive').eq(pos).addClass('liactive');
    });
      },
    applianceAccordion : function() {
    var accordionSelect = $('.accordion-appliance .acc-wrapper .acc-btn');
    accordionSelect.first().next().show();
    accordionSelect.on('click', function(){
      $(this).toggleClass('active').next().slideToggle();
    });
    },
    deleteApplianceAccordion : function() {
    var iconRemoveApp = $('.acc-btn .close-button');
    var rowAppRemove = $('.accordion-appliance .acc-wrapper');
    iconRemoveApp.on('click', function(){
      $(this).parent().parent().remove();
    });
    },
    deleteRow : function() {
    var iconRemove = $('.remove-icon');
    var rowRemove = $('.acc-text .table-appliance table tbody tr');
    iconRemove.on('click', function(){
      $(this).parent().parent().remove();
    });
    },
    activeCheck : function() {
    var selectCheck = $('.tips-box .box-img');
    var selectCheckGreen = $('.tips-box .right-img');
    selectCheck.on('click', function(){
      $(this).parent().parent().toggleClass('green'); 
    });
    selectCheckGreen.on('click', function(){
      $(this).parent().parent().toggleClass('green'); 
    });
    }
    };

  hec1.customScrollBar();
  hec1.roomSelection();
  hec1.applianceTab();
  hec1.applianceAccordion();
  hec1.deleteRow();
  hec1.activeCheck();
  hec1.deleteApplianceAccordion();

  });



})(jQuery);