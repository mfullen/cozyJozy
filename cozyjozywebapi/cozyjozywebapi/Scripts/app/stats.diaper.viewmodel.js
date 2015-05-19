function DiaperStats(app, dataModel, options) {
    var self = this;

    self.populateBarChart = function (url, selector, dateFormat) {
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

                var peeAmounts = $.map(data, function (val, i) {
                    return val.pee;
                });

                var poopAmount = $.map(data, function (val, i) {
                    return val.poop;
                });


                $.each(data, function (i, item) {

                });

                peeAmounts.unshift('Pee');
                poopAmount.unshift('Poop');

                var chart = c3.generate({
                    bindto: selector,
                    data: {
                        columns: [peeAmounts, poopAmount],
                        type: 'bar',
                        groups: [
                            ['Poop', 'Pee']
                        ]
                    },
                    color: {
                        pattern: ['#FFEB3B', '#795548', '#ff7f0e', '#ffbb78', '#2ca02c', '#98df8a', '#d62728', '#ff9896', '#9467bd', '#c5b0d5', '#8c564b', '#c49c94', '#e377c2', '#f7b6d2', '#7f7f7f', '#c7c7c7', '#bcbd22', '#dbdb8d', '#17becf', '#9edae5']
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

    self.populatePieChart = function (url, selector) {
        var colors = ['#F44336',
        '#E91E63',
        '#CDDC39',
        '#009688',
        '#9C27B0',
        '#673AB7',
        '#3F51B5',
        '#2196F3',
        '#4CAF50',
        '#FFEB3B',
        '#FFC107',
        '#FF5722',
        '#9E9E9E'];

        $.ajax({
            url: url,
            headers: dataModel.getSecurityHeaders(),
            data: { childId: app.selectedChild().child().id() },
            contentType: 'json',
            success: function (data) {
                var data2 = [];

                $.each(data, function (i, item) {
                    var color = colors[Math.floor(Math.random() * colors.length)];
                    var i = colors.indexOf(color);
                    if (i != -1) {
                        colors.splice(i, 1);
                    }
                    data2.push([item.userName + ':' + item.title, item.amount]);
                });

                var chart = c3.generate({
                    bindto: selector,
                    data: {
                        // iris data from R
                        columns: data2,
                        type: 'pie'
                    },
                    color: {
                        pattern: colors
                    }
                });


            },
            error: function (xhr, textStatus, err) {
                app.errors.push("Failed to retrieve Feeding stats. Please try again!");
                console.log("Error", xhr, textStatus, err);
            }
        });
    }



    self.populateBarChart('api/diaperstats/week/diaperovertime', '#diaperLastWeek', 'M/DD');
    self.populateBarChart('api/diaperstats/month/diaperovertime', '#diaperLastMonth', 'M/DD');
    self.populateBarChart('api/diaperstats/birth/diaperovertime', '#diaperSinceBirth', 'MMM YY');

    self.populatePieChart('api/diaperstats/most/poops', '#mostLoggedPoop');
    self.populatePieChart('api/diaperstats/most/pee', '#mostLoggedPee');

}

app.addViewModel({
    name: "DiaperStats",
    bindingMemberName: "diaperStats",
    factory: DiaperStats
});