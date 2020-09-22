$(document).ready(function () {
    var ajaxFormSubmit = function () {
        var $form = $(this);

        var options = {
            url: $form.attr("action"),
            type: $form.attr("method"),
            data: $form.serialize()
        };

        $.ajax(options).done(function (data) {
            var $target = $($form.attr("data-w-target"));
            $target.replaceWith(data);
        });

        return false;
    };

    var getPage = function () {
        var $a = $(this);

        var options = {
            url: $a.attr("href"),
            //data: $("form").serialize(),
            type: "get"
        };

        $.ajax(options).done(function (data) {
            var target = $a.parents("div.pagedList").attr("data-w-target");
            $(target).replaceWith(data);
        });
        return false;
    };

    $("form[data-w-ajax='true']").submit(ajaxFormSubmit);
    $(".body-content").on("click", ".pagedList a", getPage);

    $(".body-content").on("click", "#sortLinks a", function () {
        var $a = $(this);

        var options = {
            url: $a.attr("href"),
            type: "get"
        };

        $.ajax(options).done(function (data) {
            var target = $a.parents("div#sortLinks").attr("data-w-target");
            $(target).replaceWith(data);
        });

        return false;
    });

    $("#formId").submit(function () {
        var $form = $(this);
        var $data = "";

        var selectedBrand = $("#ddlBrand").val();
        var selectedColor = $("#ddlColor").val();
        var selectedYear = $("#ddlYear").val();
        var selectedBodyType = $("#ddlBodyType").val();

        if (selectedBrand != "") {
            $data += "Brand=" + selectedBrand;
        }
        if (selectedColor != "") {
            if ($data != "") {
                $data += "&";
            }
            $data += "Color=" + selectedColor;
        }
        if (selectedYear != "") {
            if ($data != "") {
                $data += "&";
            }
            $data += "Year=" + selectedYear;
        }
        if (selectedBodyType != "") {
            if ($data != "") {
                $data += "&";
            }
            $data += "BodyType=" + selectedBodyType;
        }

        var options = {
            url: $form.attr("action"),
            type: $form.attr("method")
        };

        if ($data != "") {
            options.data = $data;
        }

        $.ajax(options).done(function (data) {
            var $target = $($form.attr("data-w-target"));
            $target.replaceWith(data);
        });

        return false;
    });

    /*
    $("#trendingForm").submit(function () {
        var $form = $(this);
        var selectedTimeFrame = $("#ddlTime").val();

        var options = {
            url: $form.attr("action"),
            type: $form.attr("method")
        };

        if (selectedTimeFrame != "") {
            options.data = $form.serialize();
        }

        $.ajax(options).done(function (data) {
            var $target = $($form.attr("data-w-target"));
            $target.replaceWith(data);
        });

        return false;
    });*/

    //Toggling like in main page
    $(document).on("click", "a.likeIndex", function () {
        var $a = $(this);

        var options = {
            url: $a.attr("href"),
            type: "get"
        };

        $.ajax(options).done(function () {
            $a.text(($a.text() == "Like" ? "Unlike" : "Like"));
        });

        return false;
    });

    //Toggling share everywhere
    $(document).on("click", "a.shareIndex", function () {
        var $a = $(this);

        var options = {
            url: $a.attr("href"),
            type: "get"
        };

        $.ajax(options).done(function () {
            $a.text(($a.text() == "Share" ? "Unshare" : "Share"));
        });

        return false;
    });

    //Toggling like in trending page
    $(document).on("click", "a.likeTrending", function () {
        var $a = $(this);

        var options = {
            url: $a.attr("href"),
            type: "get"
        };

        $.ajax(options).done(function (data) {
            $a.text(($a.text() == "Like" ? "Unlike" : "Like"));
            $a.siblings(".allLikes").text(data);
        });

        return false;
    });
});