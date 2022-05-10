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
            $(".minicart-content-box").html(data)
        })
    })
    $(document).on("click", ".deletecard", function (e) {
        e.preventDefault();

        let url = $(this).attr("href");

        fetch(url).then(response => {
            fetch("Basket/GetBasket").then(response => response.text()).then(data => $(".minicart-content-box").html(data))

            return response.text()
        }).then(data => $(".basketContainer").html(data))
    })
    $(document).on("keyup", ".basketItemCount", function () {
        let url = $(this).next().attr("href");
        url = url + "?count=" + $(this).val();
        if (Number($(this).val().trim())) {
            fetch(url).then(response => response.text()).then(data => $(".basketContainer").html(data))
        }
        
        
    })
    $(document).on("click", ".basketUpdate", function (e) {
        e.preventDefault();
        if ((Number($(this).parent().children()[1].value))) {
            

            let url = $(this).attr("href");
            let count = $(this).parent().children()[1].value;
            count = parseInt(count);

            if ($(this).hasClass("dec")) {
                count--;
            }
            else if ($(this).hasClass("inc")) {
                count++;
            }

            $(this).parent().children()[1].value = count
            url = url + "?count=" + count;

            fetch(url).then(response => {
                fetch("Basket/GetBasket").then(response => response.text()).then(data => $(".minicart-content-box").html(data))
                return response.text()
            }).then(data => $(".basketContainer").html(data))
        }
        else {
            alert("You Can Only Number Enter")
        }
        
        
    })

    $(document).on("change", ".categoryfilter", function (e) {
        $("#filterForm").submit();
    })
  
    $(document).on("change", ".colorfilter", function (e) {
        $("#filterForm").submit();
    })
    $(document).on("change", ".sizefilter", function (e) {
        $("#filterForm").submit();
    })
    
    $(document).on("change", "#minPriceInput", function (e) {
        $("#filterForm").submit();
    })
    
    $(document).on("change", "#maxPriceInput", function (e) {
        $("#filterForm").submit();
    })
})