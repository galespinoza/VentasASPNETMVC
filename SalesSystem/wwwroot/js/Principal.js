
class Principal {
    constructor() {
        this.TotalDebt = 0.0;
    }
    userLink(URLactual) {
        var provider = new Provider();
        var shopping = new Shopping();
        let url = "";
        let cadena = URLactual.split("/");
        for (var i = 0; i < cadena.length; i++) {
            if (cadena[i] != "Index") {
                url += cadena[i];
            }
        }
        switch (url) {
            case "UsersRegister":
                document.getElementById('files').addEventListener('change', imageUser, false);
                break;
            case "CustomersRegister":
                document.getElementById('files').addEventListener('change', imageCustomer, false);
                break;
            case "CustomersReports":
                document.getElementById('inlineRadio1').checked = true;
                document.getElementById('inlineRadio2').checked = false;
                document.getElementById('inlineRadio1').disabled = false;
                document.getElementById('inlineRadio2').disabled = true;
                new Customers().SetSection(1);
                break;
            case "ProviderRegister":
                document.getElementById('files').addEventListener('change', imageProvider, false);
                break;
            case "ProviderReports":
                provider.SetSection(1);
                $('#PaymentProvider').keyup((e) => {
                    provider.Payments(e, null);
                });
                document.getElementById("inlineRadio2").checked = true;
                provider.Check();
                $('#Input_AmountFees').keyup((e) => {
                    provider.Payments(e, null);
                });
                $("#Input_AmountFees").change((e) => {
                    provider.Payments(e, null);
                });
                break;
            case "ShoppingAddShopping":
                $("#Input_Quantity").change((e) => {
                    shopping.purchaseAmount();
                });
                document.getElementById('files').addEventListener('change', imageShopping, false);
                shopping.Restore();
                break;
            case "ProductAddProduct":
                document.getElementById('files').addEventListener('change', imageProduct, false);
                break;
            case "ProductUpdateProduct":
                document.getElementById('files').addEventListener('change', updateProduct, false);
                break;
            case "BoxesBoxes":
                boxes.Check();
                break;
            case "PrincipalPrincipal":
                document.getElementById("Input_Payments").value = "";
                $('#Input_Payments').keyup((e) => {
                    this.Payments(e, null, this.TotalDebt);
                });
                break;
        }
    }
    Payments(event, input, totalDebt) {
        if (input != null) {
            if (filterFloat(event, input, totalDebt)) {
                this.Payment(event, input, totalDebt);
            } else {
                return false;
            }
        } else {
            this.Payment(event, input, totalDebt);
        }
    }
    Payment(evt, input, totalDebt) {
        var tempValue;
        this.TotalDebt = totalDebt;
        var amount = parseFloat(this.TotalDebt);
        var key = window.Event ? evt.which : evt.keyCode;
        var chark = String.fromCharCode(key);
        tempValue = input == null ? document.getElementById("Input_Payments").value
            : input.value + chark;
        if (document.getElementById("Input_Payments").value == "") {
            document.getElementById("labelVenta_Importe").innerHTML = numberDecimales(amount)
        } else {
            var payment = parseFloat(tempValue);
            var debt = amount - payment;
            if (payment > debt) {
                let change = payment - amount;
                document.getElementById("labelVenta_Change").innerHTML = numberDecimales(change);
            } else {
                document.getElementById("labelVenta_Change").innerHTML = "0.00";
            }
            let data = debt > 0 ? debt : 0.00;
            document.getElementById("labelVenta_Importe").innerHTML = numberDecimales(data)
        }

    }
}
