$(".modal-link").click(function () {
    $.get(this.href, function (data) {
        $('#modal-dialog').html(data);
    });
});

$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});

$(".images-file-input").on("change", function (e) {
    var files = e.target.files;
    var fileNames = "";
    for (var i = 0; i < files.length; i++) {
        if (i == files.length - 1) {
            fileNames += files[i].name
        }
        else {
            fileNames += files[i].name + ", "
        }
    }
    $(".imagespath-file-input").val(fileNames);
});

$(".logo-file-input").on("change", function (e) {
    $(".logopath-file-input").val(e.target.files[0].name);
});

if ($(".link-favorite").hasClass("bi-bookmark-check")) {
    $(".link-favorite").hover(
        function () {
            $(this).addClass("bi-bookmark-check-fill").removeClass("bi-bookmark-check");
        }, function () {
            $(this).removeClass("bi-bookmark-check-fill").addClass("bi-bookmark-check");
        }
    );
}
else {
    $(".link-favorite").hover(
        function () {
            $(this).removeClass("bi-bookmark-check-fill").addClass("bi-bookmark-check");
        }, function () {
            $(this).addClass("bi-bookmark-check-fill").removeClass("bi-bookmark-check");
        }
    );
}


$("#star-0").click(function (e) {
    e.preventDefault();
    $(this).removeClass('bi-star').addClass('bi-star-fill').addClass('star-fill');
    $("#star-1").removeClass('bi-star-fill').addClass('bi-star').removeClass('star-fill');
    $("#star-2").removeClass('bi-star-fill').addClass('bi-star').removeClass('star-fill');
    $("#star-3").removeClass('bi-star-fill').addClass('bi-star').removeClass('star-fill');
    $("#star-4").removeClass('bi-star-fill').addClass('bi-star').removeClass('star-fill');
    $('#ratingReview').val(1);
});

$('#star-1').click(function (e) {
    e.preventDefault();
    $("#star-0").removeClass('bi-star').addClass('bi-star-fill').addClass('star-fill');
    $(this).removeClass('bi-star').addClass('bi-star-fill').addClass('star-fill');
    $("#star-2").removeClass('bi-star-fill').addClass('bi-star').removeClass('star-fill');
    $("#star-3").removeClass('bi-star-fill').addClass('bi-star').removeClass('star-fill');
    $("#star-4").removeClass('bi-star-fill').addClass('bi-star').removeClass('star-fill');
    $('#ratingReview').val(2);
});

$('#star-2').click(function (e) {
    e.preventDefault();
    $("#star-0").removeClass('bi-star').addClass('bi-star-fill').addClass('star-fill');
    $("#star-1").removeClass('bi-star').addClass('bi-star-fill').addClass('star-fill');
    $(this).removeClass('bi-star').addClass('bi-star-fill').addClass('star-fill');
    $("#star-3").removeClass('bi-star-fill').addClass('bi-star').removeClass('star-fill');
    $("#star-4").removeClass('bi-star-fill').addClass('bi-star').removeClass('star-fill');
    $('#ratingReview').val(3);
});

$('#star-3').click(function (e) {
    e.preventDefault();
    $("#star-0").removeClass('bi-star').addClass('bi-star-fill').addClass('star-fill');
    $("#star-1").removeClass('bi-star').addClass('bi-star-fill').addClass('star-fill');
    $("#star-2").removeClass('bi-star').addClass('bi-star-fill').addClass('star-fill');
    $(this).removeClass('bi-star').addClass('bi-star-fill').addClass('star-fill');
    $("#star-4").removeClass('bi-star-fill').addClass('bi-star').removeClass('star-fill');
    $('#ratingReview').val(4);
});

$('#star-4').click(function (e) {
    e.preventDefault();
    $("#star-0").removeClass('bi-star').addClass('bi-star-fill').addClass('star-fill');
    $("#star-1").removeClass('bi-star').addClass('bi-star-fill').addClass('star-fill');
    $("#star-2").removeClass('bi-star').addClass('bi-star-fill').addClass('star-fill');
    $("#star-3").removeClass('bi-star').addClass('bi-star-fill').addClass('star-fill');
    $(this).removeClass('bi-star').addClass('bi-star-fill').addClass('star-fill');
    $('#ratingReview').val(5);
});

$(document).ready(function () {
    $('.slider').slick({
        infinite: true,
        slidesToShow: 4,
        slidesToScroll: 1
    });
    $('.business-slider').slick({
        dots: true,
        fade: true
    });
    $('.category-slider').slick({
        infinite: true,
        slidesToShow: 6,
        slidesToScroll: 1
    });
    $('.workers-slider').slick({
        infinite: true,
        slidesToShow: 6,
        slidesToScroll: 1
    });
});

$(".time-button").click(function () {
    $("#schedule-id").val($(this).val());
    $("#record-time").val($(this).children('div').text().trim());
});