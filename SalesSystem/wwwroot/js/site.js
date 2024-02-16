// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var principal = new Principal();

/*CODIGO DE USUARIOS*/
var user = new User();
var imageUser = (evt) => {
    user.archivo(evt, "imageUser");
}
/*CODIGO DE CLIENTES*/
var customers = new Customers();
var imageCustomer = (evt) => {
    customers.archivo(evt, "imageCustomer");
}
/*CODIGO DE Proveedor*/
var provider = new Provider();
var imageProvider = (evt) => {
    provider.archivo(evt, "imageProvider");
}

/*CODIGO DE COMPRAS*/
var shopping = new Shopping();
var imageShopping = (evt) => {
    shopping.archivo(evt, "imageShopping");
}

/*CODIGO DE PRODUCTOS*/
var product = new Product();
var imageProduct = (evt) => {
    product.archivo(evt, "imageProduct");
}

var updateProduct = (evt) => {
    product.archivo(evt, "updateProduct");
}

/*CODIGO DE BOXES*/
var boxes = new Boxes();


$().ready(() => {
    let URLactual = window.location.pathname;
    principal.userLink(URLactual);

    $("#Input_AmountFees").change((e) => {
        let idClient = window.location.search.replace("?id=", "");
        customers.GetInterests(e, null, idClient);
    });
    $('#Input_AmountFees').keyup((e) => {
        var key = e.which || e.keyCode || e.charCode;
        if (key == 8) {
            let idClient = window.location.search.replace("?id=", "");
            customers.GetInterests(e, null, idClient);

        }
        return true;
    });
    $('#Input_Payment').keyup((e) => {
        var key = e.which || e.keyCode || e.charCode;
        if (key == 8) {
            customers.Payments(e, null);
        }
        return true;
    });

    /*CODIGO DE Proveedor*/


    /*CODIGO DE COMPRAS*/

});