class Shopping extends Uploadpicture {
    constructor() {
        super();
        this.URL = window.location.origin;
    }
    purchaseAmount() {
        var Quantity = document.getElementById("Input_Quantity").value;

        var Price = document.getElementById("Input_Price").value;
        let Amount = Price * Quantity;
        document.getElementById("labelCompra_Importe").innerHTML = numberDecimales(Amount);
    }
    Payments(amount) {
        var payments = document.getElementById("Input_Payments").value
        if (payments != "000") {
            let debt = amount - payments;
            if (amount < payments) {
                $('#payment').attr("disabled", false);
                $('#check').attr("disabled", true);
                document.getElementById("labelCompra_Debt").innerHTML = "Cambio para el sistema";
            } else if (amount == payments) {
                $('#check').attr("disabled", true);
                $('#payment').attr("disabled", false);
            } else {
                //$('#check').attr("disabled", false);
                //$('#payment').attr("disabled", true);
            }
            let debts = numberDecimales(debt);
            document.getElementById("labelCompra_Debts").innerHTML = debts.replace('-', '');
        } else {
            if (document.getElementById("check").checked) {
                $('#Input_Payments').attr("disabled", true);
                $('#payment').attr("disabled", false);
            }
        }

    }
    Check() {
        var payments = document.getElementById("Input_Payments").value;
        if (document.getElementById("check").checked) {
            $('#payment').attr("disabled", false);
            if (payments == "000") {
                $('#Input_Payments').attr("disabled", true);
            }
        } else {
            $('#payment').attr("disabled", true);
            $('#Input_Payments').attr("disabled", false);
        }
    }
    GetProvider() {
        var provider = document.getElementById("Input_Provider").value;
        $.post(
            this.URL + "/Shopping/GetProvider",
            { provider },
            (response) =>{
                $("#listProvider").html(response[0]);
            }
        );
    }
    SetProvider() {
        let providers = document.getElementById("listProvider");
        let provider = providers.options[providers.selectedIndex].text;
        document.getElementById("Input_Provider").value = provider;
    }
    Restore() {
        $('#payment').attr("disabled", true);
        document.getElementById("Input_Payments").value = "";
        this.purchaseAmount();
        this.GetProvider();
    }
}