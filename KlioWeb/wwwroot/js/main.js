(function ($) {
	"use strict";

	$(window).on('load', function () {
		aosAnimation();
	});

	/*=============================================
	=          Data Background		              =
	=============================================*/
	$("[data-background]").each(function () {
		$(this).css("background-image", "url(" + $(this).attr("data-background") + ")")
	})

	/*=============================================
	=    		Mobile Menu			              =
	=============================================*/
	//SubMenu Dropdown Toggle
	if ($('.menu-area li.menu-item-has-children ul').length) {
		$('.menu-area .navigation li.menu-item-has-children').append('<div class="dropdown-btn"><span class="fas fa-angle-down"></span></div>');
	}
	//Mobile Nav Hide Show
	if ($('.mobile-menu').length) {

		var mobileMenuContent = $('.menu-area .main-menu').html();
		$('.mobile-menu .menu-box .menu-outer').append(mobileMenuContent);

		//Dropdown Button
		$('.mobile-menu li.menu-item-has-children .dropdown-btn').on('click', function () {
			$(this).toggleClass('open');
			$(this).prev('ul').slideToggle(500);
		});
		//Menu Toggle Btn
		$('.mobile-nav-toggler').on('click', function () {
			$('body').addClass('mobile-menu-visible');
		});

		//Menu Toggle Btn
		$('.menu-backdrop, .mobile-menu .close-btn').on('click', function () {
			$('body').removeClass('mobile-menu-visible');
		});
	}

	/*=============================================
	=     Menu sticky & Scroll to top			  =
	=============================================*/
	$(window).on('scroll', function () {
		var scroll = $(window).scrollTop();
		if (scroll < 245) {
			$("#sticky-header").removeClass("sticky-menu");
			$('.scroll-to-target').removeClass('open');

		} else {
			$("#sticky-header").addClass("sticky-menu");
			$('.scroll-to-target').addClass('open');
		}
	});

	/*=============================================
	=    		 Scroll Up  	                  =
	=============================================*/
	if ($('.scroll-to-target').length) {
		$(".scroll-to-target").on('click', function () {
			var target = $(this).attr('data-target');
			// animate
			$('html, body').animate({
				scrollTop: $(target).offset().top
			}, 1000);

		});
	}

	/*=============================================
	=       Most Popular Partitions Active        =
    =============================================*/

	var owl = $('.owl-carousel').owlCarousel({
		loop: false,
		items: 3,
		autoplay: false,
		autoplayTimeout: 5000,
		autoplaySpeed: 1000,
		nav: true,
		navText: ['<i class="fa-solid fa-chevron-left"></i>', '<i class="fa-solid fa-chevron-right"></i>'],
		navigation: true,
		navigationText: ['<i class="fa-solid fa-chevron-left"></i>', '<i class="fa-solid fa-chevron-right"></i>'],
		dots: false,
		responsive: {
			0: {
				items: 1,
				nav: false,
			},
			575: {
				items: 2,
				nav: false,
			},
			768: {
				items: 2,
				nav: false,
			},
			992: {
				items: 3,
			},
			1200: {
				items: 3
			},
		}
	});

	$('.owl-filter-bar').on('click', '.item', function () {
		$('.owl-filter-bar .item').removeClass("active");

		var $item = $(this);
		var filter = $item.data('owl-filter')

		$item.addClass("active");

		owl.owlcarousel2_filter(filter);

	});

	/*=============================================
	=    		 Aos Active  	                  =
    =============================================*/
	function aosAnimation() {
		AOS.init({
			duration: 1000,
			mirror: true,
			once: true,
			disable: 'mobile',
		});
	}
})(jQuery);