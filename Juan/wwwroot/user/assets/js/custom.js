$(document).ready(function () {
    $(document).on("click", ".productdetail", function (e) {
        e.preventDefault();
        let url = $(this).attr("href");

        fetch(url).then(response => response.text())
            .then(data => {
                $(".modal-content").html(data)

                $('.product-large-slider').slick({
                    fade: true,
                    arrows: false,
                    asNavFor: '.pro-nav'
                });

               
                // product details slider nav active
                $('.pro-nav').slick({
                    slidesToShow: 4,
                    asNavFor: '.product-large-slider',
                    arrows: false,
                    focusOnSelect: true
                });

                $('.pro-qty').prepend('<span class="dec qtybtn">-</span>');
                $('.pro-qty').append('<span class="inc qtybtn">+</span>');
                $('.qtybtn').on('click', function () {
                    var $button = $(this);
                    var oldValue = $button.parent().find('input').val();
                    if ($button.hasClass('inc')) {
                        var newVal = parseFloat(oldValue) + 1;
                    } else {
                        // Don't allow decrementing below zero
                        if (oldValue > 0) {
                            var newVal = parseFloat(oldValue) - 1;
                        } else {
                            newVal = 0;
                        }
                    }
                    $button.parent().find('input').val(newVal);
                });
                $('.img-zoom').zoom();
                $("#productQuickModal").modal("show")
            })
    })
    $(document).on("click", ".addbasketbtn", function (e) {
        e.preventDefault()
        console.log("ok")
        let selectColor = $(".color").val();
        let selectSize = $(".size").val();
        let url = $("#basketform").attr("action")
        let count = $("#productcount").val();
        url = url + "?count=" + count+"&colorId="+selectColor+"&sizeId="+selectSize;
        fetch(url).then(response => {
            return response.text();
        }).then(data => {
            /*$(".header-cart").html(data)*/
            console.log(data)

        })
    })
})