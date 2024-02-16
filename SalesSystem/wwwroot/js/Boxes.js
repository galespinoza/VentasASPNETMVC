class Boxes {
    Check() {
        if (document.getElementById("check").checked) {
            $('#BoxeRegister').attr("disabled", true);
            $('#Input_Box').attr("disabled", true);
            $('#State').attr("disabled", true);

            $('#Input_Ticket').attr("disabled", false);
            $('#Input_Money').attr("disabled", false);
            $('#buttonBox').attr("disabled", false);
        } else {
            $('#BoxeRegister').attr("disabled", false);
            $('#Input_Box').attr("disabled", false);
            $('#State').attr("disabled", false);

            $('#Input_Ticket').attr("disabled", true);
            $('#Input_Money').attr("disabled", true);
            $('#buttonBox').attr("disabled", true);
            document.getElementById("dangerTicket").innerHTML = "";
            document.getElementById("dangerMoney").innerHTML = "";
        }
    }
}