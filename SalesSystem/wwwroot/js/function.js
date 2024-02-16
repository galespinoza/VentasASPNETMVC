
var numberDecimales = (number) => {
    return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
var filterFloat = (evt, input) => {
    if (input != null) {
        // Backspace = 8, Enter = 13, ‘0′ = 48, ‘9′ = 57, ‘.’ = 46, ‘-’ = 43
        var key = window.Event ? evt.which : evt.keyCode;
        var chark = String.fromCharCode(key);
        var tempValue = input.value + chark;
        if (key >= 48 && key <= 57) {
            if (filter(tempValue) === false) {
                return false;
            } else {
                return true;
            }
        } else {
            if (key == 8 || key == 13 || key == 0) {
                return true;
            } else if (key == 46) {
                if (filter(tempValue) === false) {
                    return false;
                } else {
                    return true;
                }
            } else {
                return false;
            }
        }
    }
}
var filter = (__val__) => {
    var preg = /^([0-9]+\.?[0-9]{0,2})$/;
    return preg.test(__val__);
}