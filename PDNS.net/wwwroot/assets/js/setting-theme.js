"use strict";

// Setting Color

$(window).resize(function() {
	$(window).width(); 
});

$('.changeBodyBackgroundFullColor').on('click', function(){
	if($(this).attr('data-color') == 'default'){
		$('body').removeAttr('data-background-full');
	} else {
		$('body').attr('data-background-full', $(this).attr('data-color'));
	}

	$(this).parent().find('.changeBodyBackgroundFullColor').removeClass("selected");
	$(this).addClass("selected");
	layoutsColors();
});

$('.changeLogoHeaderColor').on('click', function(){
	if($(this).attr('data-color') == 'default'){
		$('.logo-header').removeAttr('data-background-color');
	} else {
		$('.logo-header').attr('data-background-color', $(this).attr('data-color'));
	}

	$(this).parent().find('.changeLogoHeaderColor').removeClass("selected");
	$(this).addClass("selected");
	layoutsColors();
});

$('.changeTopBarColor').on('click', function(){
	if($(this).attr('data-color') == 'default'){
		$('.main-header .navbar-header').removeAttr('data-background-color');
	} else {
		$('.main-header .navbar-header').attr('data-background-color', $(this).attr('data-color'));
	}

	$(this).parent().find('.changeTopBarColor').removeClass("selected");
	$(this).addClass("selected");
	layoutsColors();
});

$('.changeSideBarColor').on('click', function(){
	if($(this).attr('data-color') == 'default'){
		$('.sidebar').removeAttr('data-background-color');
	} else {
		$('.sidebar').attr('data-background-color', $(this).attr('data-color'));
	}

	$(this).parent().find('.changeSideBarColor').removeClass("selected");
	$(this).addClass("selected");
	layoutsColors();
});

$('.changeBackgroundColor').on('click', function(){
	$('body').removeAttr('data-background-color');
	$('body').attr('data-background-color', $(this).attr('data-color'));
	$(this).parent().find('.changeBackgroundColor').removeClass("selected");
	$(this).addClass("selected");
});

$('.changeFooterColor').on('click', function(){
	$('footer.footer').removeAttr('data-background-color');
	$('footer.footer').attr('data-background-color', $(this).attr('data-color'));
	$(this).parent().find('.changeFooterColor').removeClass("selected");
	$(this).addClass("selected");
});

$('.changeTSwitchColor').on('click', function () {
	$('.theme-content').removeAttr('data-background-color');
	$('.theme-content').attr('data-background-color', $(this).attr('data-color'));
	$(this).parent().find('.changeTSwitchColor').removeClass("selected");
	$(this).addClass("selected");
});


var toggle_customSidebar = false,
custom_open = 0;

if(!toggle_customSidebar) {
	var toggle = $('.theme-panel .theme-toggle');

	toggle.on('click', (function(){
		if (custom_open == 1){
			$('.theme-panel').removeClass('open');
			toggle.removeClass('toggled');
			custom_open = 0;
		}  else {
			$('.theme-panel').addClass('open');
			toggle.addClass('toggled');
			custom_open = 1;
		}
	})
	);
	toggle_customSidebar = true;
}