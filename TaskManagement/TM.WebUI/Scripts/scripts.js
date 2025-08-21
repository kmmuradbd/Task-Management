$(document).ready(function (e) {
  $('.navbar-toggler').on('click', function (e) {
    // $('body').toggleClass('in');
    $('.navbar-collapse').toggleClass('show');
    $(this).toggleClass('open');
    e.stopPropagation();
  });
  $('body').on('click', function () {
    $('.navbar-toggler').removeClass('open');
    // $('body').removeClass('in');
    $('.navbar-collapse').removeClass('show');
  });
  $(".navbar-collapse").on('click', function (e) {
    e.stopPropagation();
  });
  $('ul li:has(ul)').addClass('dropdown');

  $('.navbar-collapse').on('click', '.dropdown a', function () {
    if ($(this).width() < 800) {
      if ($(this).next('ul').is(':visible')) {
        $(this).next('ul').slideUp('fast');
        $(this).removeClass('active');
      } else {
        $(this).closest('ul').children('.dropdown').children('.active').next('ul').slideUp('fast');
        $(this).closest('ul').children('.dropdown').children('.active').removeClass('active');
        $(this).next().slideToggle('fast');
        $(this).addClass('active');
      }
    }
  });

  // $("body").niceScroll();
});

// Modal
function showModal(id) {
  $('#' + id).fadeIn(400);
}

function hideModal(id) {
  $('#' + id).fadeOut(400);
}

// TAB
function slideItem(thechosenone) {
  $('.slide-content').each(function (index) {
    if ($(this).attr("id") == thechosenone) {
      $(this).slideDown(500);
    } else {
      $(this).slideUp(500);
    }
  });
}
$('.slide-nav ul li a').click(function () {
  $('.slide-nav ul li a').removeClass('active');
  $(this).addClass('active');
});
// TAB END

