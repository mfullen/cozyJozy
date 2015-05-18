function FeedingStats(app, dataModel, options) {
    var self = this;


    self.populateLineChart = function(url, selector, dateFormat, fillColor, converter, opts) {
        $.ajax({
            url: url,
            //cache: false,
            headers: dataModel.getSecurityHeaders(),
            data: { childId: app.selectedChild().child().id() },
            contentType: 'json',
            success: function (data) {
                var labels = $.map(data, function (val, i) {
                    // Do something
                    return moment(val.date).format(dateFormat);
                });

                var amounts = $.map(data, function (val, i) {
                    // Do something
                    if (converter) {
                        return converter(val.amount);
                    }
                    return val.amount;
                });
  
                var data2 = {
                    labels: labels,
                    datasets: [
                        {
                            label: "My First dataset",
                            fillColor: fillColor,//"rgba(220,220,220,0.2)",
                            strokeColor: "rgba(220,220,220,1)",
                            pointColor: "rgba(220,220,220,1)",
                            pointStrokeColor: "#000",
                            pointHighlightFill: "#000",
                            pointHighlightStroke: "rgba(220,220,220,1)",
                            data: amounts
                        }
                    ]
                };


                // Get context with jQuery - using jQuery's .get() method.
                var ctx = $(selector).get(0).getContext("2d");
                // This will get the first returned node in the jQuery collection.
                var myNewChart = new Chart(ctx);


                var width = $('canvas').parent().width();
                $('canvas').attr("width", width);
                myNewChart.Line(data2, opts);
                window.onresize = function (event) {
                    var width = $('canvas').parent().width();
                    $('canvas').attr("width", width);
                    myNewChart.Line(data2, opts);
                };

                myNewChart.Line(data2, opts);
            },
            error: function (xhr, textStatus, err) {
                app.errors.push("Failed to retrieve Feeding stats. Please try again!");
                console.log("Error", xhr, textStatus, err);
            }
        });
    }


    self.populateLineChart('api/feedingstats/birth/feedovertime', '#sinceBirth', 'MMM YY', '#4CAF50', convertMlToOz, {
        scaleLabel: '<%=value + " oz"%>'
    });

    self.populateLineChart('api/feedingstats/week/feedovertime', '#feedAmountLastWeek', 'M/DD', '#FF9800', convertMlToOz, {
        scaleLabel: '<%=value + " oz"%>'
    });

    self.populateLineChart('api/feedingstats/3month/feedovertime', '#3Month', 'MMM YY', '#03A9F4', convertMlToOz, {
        scaleLabel: '<%=value + " oz"%>'
    });


    self.populateLineChart('api/feedingstats/week/feedcount', '#feedCountLastWeek', 'M/DD', '#607D8B');
    self.populateLineChart('api/feedingstats/month/feedcount', '#feedCountLastMonth', 'M/DD', '#CDDC39');

}

app.addViewModel({
    name: "FeedingStats",
    bindingMemberName: "feedingStats",
    factory: FeedingStats
});