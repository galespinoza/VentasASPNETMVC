class Customers extends Uploadpicture {

    SetSection(value) {
        switch (value) {
            case 1:
                document.getElementById('inlineRadio1').checked = true;
                document.getElementById('inlineRadio2').checked = false;
                document.getElementById('inlineRadio1').disabled = false;
                document.getElementById('inlineRadio2').disabled = true;
                localStorage.setItem("section", value);
                this.Restore();
                break; 
            case 2:
                document.getElementById('inlineRadio2').checked = true;
                document.getElementById('inlineRadio1').checked = false;
                document.getElementById('inlineRadio2').disabled = false;
                document.getElementById('inlineRadio1').disabled = true;
                localStorage.setItem("section", value);
                this.Restore();
                break;
        }
    }
    GetInterests(event, input, idClient) {
        var fees = 0;
        var key = window.Event ? event.which : event.keyCode;
        var chark = String.fromCharCode(key);
        if (input == null) {
            fees = document.getElementById("Input_AmountFees").value;
        } else {
            fees = input.value + chark;
        }
        $.post(
            window.location.origin + "/Customers/Fees?area=Customers",
            { fees: fees, idClient: idClient },
            (response) => {
                document.getElementById("amountFees").innerHTML = response
                localStorage.setItem("payment", response);

            }
        );
    }
    Payments(event, input) {
        var tempValue;
        var key = window.Event ? event.which : event.keyCode;
        var chark = String.fromCharCode(key);
        if (input == null) {
            tempValue = document.getElementById("Input_Payment").value;
        } else {
            tempValue = input.value + chark;
        }
        var payment1 = parseFloat(tempValue);
        let section = parseInt(localStorage.getItem("section"));
        switch (section) {
            case 1:
                let monthly = parseFloat(document.getElementById("monthly").value);
                if (payment1 >= monthly) {
                    if (payment1 > monthly) {
                        let change = payment1 - monthly;
                        let value = "El cambio para el cliente es: " + numberDecimales(change);
                        document.getElementById("paymentMessage").innerHTML = value;
                    }
                    $('#payment').attr("disabled", false);
                } else {
                    $('#payment').attr("disabled", true);
                    document.getElementById("paymentMessage").innerHTML = "";
                }
                break;
            case 2:
                var payment2 = parseFloat(localStorage.getItem("payment"));
                if (payment1 >= payment2) {
                    if (payment1 > payment2) {
                        let change = payment1 - payment2;
                        let value = "El cambio para el cliente es: " + numberDecimales(change);
                        document.getElementById("paymentMessage").innerHTML = value;
                    }
                    $('#payment').attr("disabled", false);
                } else {
                    $('#payment').attr("disabled", true);
                    document.getElementById("paymentMessage").innerHTML = "";
                }
                break;
        }
    }
    Restore() {
        document.getElementById("Input_AmountFees").value = 0;
        document.getElementById("amountFees").innerHTML = "";
        document.getElementById("Input_Payment").value = "";
        document.getElementById("paymentMessage").innerHTML = "";
        $('#payment').attr("disabled", true);
    }
}
