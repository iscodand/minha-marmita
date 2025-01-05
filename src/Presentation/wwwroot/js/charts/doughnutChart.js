const generateDoughnutChart = (cssClass, data, labels) => {

    return new Chart(document.getElementById(cssClass), {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                label: 'Quantidade',
                data: data,
                borderWidth: 1,
                backgroundColor: [
                    '#6dae3f',
                    '#AE3838',
                    '#cfcfcf'
                ],
            }],
        },
    });
}

const generateDoughnutChartCustomColor = (cssClass, data, labels) => {
    return new Chart(document.getElementById(cssClass), {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                label: 'Quantidade de Pedidos',
                data: data,
                borderWidth: 1
            }],
        },
    });
}