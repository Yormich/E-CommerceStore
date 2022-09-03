var rangeEl = document.getElementById('inputRange');
var displayRange = document.getElementById('outputRange')
rangeEl.addEventListener('input', function () {
    displayRange.innerHTML = rangeEl.value;
}, false);