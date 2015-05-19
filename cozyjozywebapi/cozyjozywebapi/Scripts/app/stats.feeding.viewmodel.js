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
  
                amounts.unshift('Amount oz');

                var chart = c3.generate({
                    bindto: selector,
                    data: {
                        columns: [amounts]
                    },
                    axis: {
                        x: {
                            type: 'category',
                            categories: labels
                        }
                    }
                });
              
            },
            error: function (xhr, textStatus, err) {
                app.errors.push("Failed to retrieve Feeding stats. Please try again!");
                console.log("Error", xhr, textStatus, err);
            }
        });
    }


    self.populateLineChart('api/feedingstats/birth/feedovertime', '#sinceBirth', 'MMM YY', '#4CAF50', convertMlToOz);

    self.populateLineChart('api/feedingstats/week/feedovertime', '#feedAmountLastWeek', 'M/DD', '#FF9800', convertMlToOz);

    self.populateLineChart('api/feedingstats/3month/feedovertime', '#lastThreeMonths', 'MMM YY', '#03A9F4', convertMlToOz);


    self.populateLineChart('api/feedingstats/week/feedcount', '#feedCountLastWeek', 'M/DD', '#607D8B');
    self.populateLineChart('api/feedingstats/month/feedcount', '#feedCountLastMonth', 'M/DD', '#CDDC39');

}

app.addViewModel({
    name: "FeedingStats",
    bindingMemberName: "feedingStats",
    factory: FeedingStats
});