class Provider extends Uploadpicture {

    SetSection(value) {
        switch (value) {
            case 1:
                document.getElementById('inlineRadio1').checked = true;
                document.getElementById('inlineRadio1').disabled = false;
                break;
            case 2:
                document.getElementById('inlineRadio2').checked = true;
                this.Check();
                break;
        }
        localStorage.setItem("section", value);
        document.getElementById("section").value = value;
        this.Restore();
    }
    Payments(event, input) {
        if (input != null) {
            if (filterFloat(event, input)) {
                this.Payment(event, input);
            } else {
                return false;
            }
        } else {
            this.Payment(event, input);
        }
    }
    Payment(event, input) {
        var tempValue;
        var key = window.Event ? event.which : event.keyCode;
        var chark = String.fromCharCode(key);
        if (input == null) {
            tempValue = document.getElementById("PaymentProvider").value;
        } else {
            tempValue = input.value + chark;
        }
        var payment1 = parseFloat(tempValue);
        let section = parseInt(localStorage.getItem("section"));
        var value2 = document.getElementById("currentDebt").value;
        var debt = parseFloat(value2);
        switch (section) {
            case 1:
                var value1 = document.getElementById("monthly").value;
                if (value1 != "0") {
                    var fees = document.getElementById("Input_AmountFees").value;
                    if (fees > 0) {

                        var monthly = parseFloat(value1);
                        if (debt > 0) {
                            var valor1 = Math.ceil(debt / monthly);
                            var coutas = parseInt(Math.ceil(valor1));
                            if (coutas >= fees) {
                                monthly = monthly * fees;
                                if (payment1 >= monthly) {
                                    if (payment1 > monthly) {
                                        let change = payment1 - monthly;
                                        let value = "Cambio para el sistema" + numberDecimales(change);
                                        document.getElementById("paymentMessage").innerHTML = value
                                        //$('#payment').attr("disabled", true);
                                    }
                                    $('#payment').attr("disabled", false);
                                    if (payment1 == monthly) {
                                        document.getElementById("paymentMessage").innerHTML = "";

                                    }
                                } else {
                                    if (document.getElementById("PaymentProvider").value != "") {
                                        let importe = monthly - payment1;
                                        let value = "El importe faltante es: " + numberDecimales(importe);
                                        document.getElementById("paymentMessage").innerHTML = value;
                                    } else {
                                        document.getElementById("paymentMessage").innerHTML = "";
                                    }

                                    $('#payment').attr("disabled", true);

                                }
                            } else {
                                let value = "Se sobrepaso de las cuotas a pagar";
                                document.getElementById("paymentMessage").innerHTML = value;
                            }
                        } else {
                            let value = "El sistema no contiene deuda con el proveedor";
                            document.getElementById("paymentMessage").innerHTML = value;

                        }
                    } else {
                        document.getElementById("paymentMessage").innerHTML = "Seleccione la cantidad de cuotas a pagar";
                    }
                } else {
                    document.getElementById("paymentMessage").innerHTML = "Establezca una forma de pago";
                    $('#payment').attr("disabled", true);
                }
                break;
            case 2:
                if (document.getElementById("PaymentProvider").value == "" || payment1 <= 0) {
                    $('#payment').attr("disabled", true);
                    document.getElementById("fee").innerHTML = 0;
                } else {
                    var valor2 = Math.ceil(debt / payment1);
                    var coutas1 = parseInt(Math.ceil(valor2));
                    document.getElementById("fee").innerHTML = coutas1;
                    $('#payment').attr("disabled", false);
                }
                break;
        }
    }
    Check() {

        if (document.getElementById("inlineRadio2").checked) {
            document.getElementById("fees").innerHTML = "Quincenal";
        } else {
            document.getElementById("fees").innerHTML = "Mensual";
        }
    }
    Restore() {
        document.getElementById("PaymentProvider").value = "";
        document.getElementById("paymentMessage").innerHTML = "";
        $('#payment').attr("disabled", true);
    }
}