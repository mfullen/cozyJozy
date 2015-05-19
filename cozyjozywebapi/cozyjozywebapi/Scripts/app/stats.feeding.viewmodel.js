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
  
                var svg = dimple.newSvg(selector, 800, 600);
                var data2 = [
                  { "Word": "Hello", "Awesomeness": 2000 },
                  { "Word": "World", "Awesomeness": 3000 }
                ];
                var chart = new dimple.chart(svg, data2);
                chart.addCategoryAxis("x", "Word");
                chart.addMeasureAxis("y", "Awesomeness");
                chart.addSeries(null, dimple.plot.bar);
                chart.draw();
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

    self.populateLineChart('api/feedingstats/3month/feedovertime', '#lastThreeMonths', 'MMM YY', '#03A9F4', convertMlToOz, {
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