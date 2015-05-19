function DiaperStats(app, dataModel, options) {
    var self = this;

    self.populateBarChart = function (url, selector, dateFormat, fillColor, converter, opts) {
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

    self.populatePieChart = function (url, selector, dateFormat, fillColor, converter, opts, legendDiv) {
        var colors = ['#F44336',
        '#E91E63',
        '#9C27B0',
        '#673AB7',
        '#3F51B5',
        '#2196F3',
        '#009688',
        '#4CAF50',
        '#CDDC39',
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
                    data2.push({
                        value: item.amount,
                        color: color,
                        highlight: color,
                        label: item.userName + ':' + item.title,
                    });
                });

                // Get context with jQuery - using jQuery's .get() method.
                var ctx = $(selector).get(0).getContext("2d");
                // This will get the first returned node in the jQuery collection.
                var myNewChart = new Chart(ctx);

                var myPieChart = myNewChart.Pie(data2, opts);
                var legend = myPieChart.generateLegend();
                $(legendDiv).html(legend);
            },
            error: function (xhr, textStatus, err) {
                app.errors.push("Failed to retrieve Feeding stats. Please try again!");
                console.log("Error", xhr, textStatus, err);
            }
        });
    }



    self.populateBarChart('api/diaperstats/week/diaperovertime', '#diaperLastWeek', 'M/DD', '#607D8B', null, {
        legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].fillColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>"
    });
    self.populateBarChart('api/diaperstats/month/diaperovertime', '#diaperLastMonth', 'M/DD', '#CDDC39');
    self.populateBarChart('api/diaperstats/birth/diaperovertime', '#diaperSinceBirth', 'MMM YY', '#CDDC39');

    //self.populatePieChart('api/diaperstats/most/poops', '#mostLoggedPoop', 'MMM YY', '#CDDC39', null, {
    //    legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<segments.length; i++){%><li><span style=\"background-color:<%=segments[i].fillColor%>\"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>"
    //},
    //'#mostPooplegendDiv');
    //self.populatePieChart('api/diaperstats/most/pee', '#mostLoggedPee', 'MMM YY', '#CDDC39', null, {
    //    legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<segments.length; i++){%><li><span style=\"background-color:<%=segments[i].fillColor%>\"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>"
    //},
    //'#mostPeelegendDiv');

}

app.addViewModel({
    name: "DiaperStats",
    bindingMemberName: "diaperStats",
    factory: DiaperStats
});