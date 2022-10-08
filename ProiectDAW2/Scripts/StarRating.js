
$(function () {

    var $rateYo = $("#rateYo").rateYo();
    $("#getRating").click(function () {

        var rating = $rateYo.rateYo("rating");
        var idul = document.getElementById("div_rating").innerHTML;
        $.ajax({
            type: "POST",
            url: '/Ratings/New',
            data: {
                'RatingDat': rating,
                'IdulProdus': idul
            },
        });
    });
}
);
