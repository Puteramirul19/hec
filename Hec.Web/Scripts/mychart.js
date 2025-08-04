var chart = new Chartist.Line('.ct-chart', {
  labels: ['Jan', 'Feb', 'Mac', 'Apr', 'May', 'June', 'July', 'Aug'],
  // Naming the series with the series object array notation
  series: [{
    name: 'series-1',
    data: [55, 70, 62, 63, 72, 61, 64, 53]
  }, {
    name: 'series-2',
    data: [47, 52, 58, 49, 64, 58, 52, 46]
  }, {
    name: 'series-3',
    data: [34, 52, 51, 47, 50, 52, 46, 34]
  }]
}, {

  fullWidth: true,

  // Within the series options you can use the series names
  // to specify configuration that will only be used for the
  // specific series.
  series: {
    'series-1': {
      lineSmooth: false,
    },
    'series-2': {
      lineSmooth: false,
    },
    'series-3': {
      lineSmooth: false,
    }
  },

  
}, [
  // You can even use responsive configuration overrides to
  // customize your series configuration even further!
  ['screen and (max-width: 320px)', {
    series: {
      'series-1': {
        lineSmooth: Chartist.Interpolation.none()
      },
      'series-2': {
        lineSmooth: Chartist.Interpolation.none(),
        showArea: false
      },
      'series-3': {
        lineSmooth: Chartist.Interpolation.none(),
        showPoint: true
      }
    }
  }],


]);