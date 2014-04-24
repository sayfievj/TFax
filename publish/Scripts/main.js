$(function () {

    //menu 
    $("ul#site-navbar").find("li[data-url='" + $("ul#site-navbar").data("url") + "']").addClass("active");

    //layout 
    $("h1.site-title").click(function () {
        var self = $(this);

        window.location.href = self.data("url");
    });

    //account
    $(":checkbox#iagree").change(function () {
        var self = $(this);
        var submit = $(":submit#btn-sign-up");

        if (self.is(':checked')) {
            submit.removeClass("disabled");
        }
        else {
            submit.addClass("disabled");
        }

    });

    //fancybox 
    $("a[href^='data:image']").each(function () {
        $(this).fancybox({
            content: $("<img/>").attr("src", this.href)
        });
    });

    //sign up
    $(":submit#btn-sign-up").addClass("disabled");
      
    $('#banner-fade').bjqs({
        height: 340,
        width: 825,
        responsive: true,
        animtype: 'fade',
        animduration: 500,
        animspeed: 6000,
    });

    /* equal height opera glitch */
    $('.c-wrap').each(function () {
        var highestBox = 0;
        $('.spread', this).each(function () {
            if ($(this).height() > highestBox)
                highestBox = $(this).height()/* + 100*/;
        });
        $('.spread-side', this).height(highestBox);
    });


    //selectbox
    $('.selectbox2').selectBox({
        mobile: true,
        menuSpeed: 'fast',
        menuTransition: 'fade',
        keepInViewport: false
    });


    /*This has to come last - it has a glitch */
    $('.split-six').easyListSplitter({
        colNumber: 6
    });

});