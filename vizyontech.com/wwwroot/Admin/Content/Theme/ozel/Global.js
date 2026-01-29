function ModalBox(Message, Redirect, Class, NotfyAlert, BootBoxAlert) {
    if (NotfyAlert) {
        $.notify(Message, Class);
        if (Redirect != "") {

            setTimeout(function () { window.location = Redirect; }, 1000);

            // window.location.href = Redirect;
        }
    }
    if (BootBoxAlert) {
        bootbox.alert(Message, function () {
            if (Redirect != "") { window.location.href = Redirect; }
        });
    }

}

function FancyBoxIframe() {
    $.fancybox.open({
        href: '/',
        type: 'iframe',
        width: 450,
        minHeight: 470,
        padding: 5,
        closeClick: false,
        onClosed: function () {
            jQuery.fancybox.close();
        },
        afterClose: function () {
            jQuery.fancybox.close();
        },
        mouseWheel: false
    });
}



function Sil(Id, Controller, Action) {
    try {
        bootbox.confirm({
            message: "Silmek istediğinize eminmisiniz ?",
            buttons: {
                confirm: {
                    label: 'Evet',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'Hayır',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result) {
                    var dataValue = Id;
                    $.ajax({
                        url: "/Admin/" + Controller + "/" + Action + "",
                        data: { Id: dataValue },
                        type: "POST",
                        dataType: "json",
                        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                        success: function (data, status, xhr) {
                            if (data.Basarilimi) {
                                ModalBox(data.Mesaj, "", "success", data.NotfyAlert, data.BootBoxAlert);

                                $(".Liste_" + Id).css('background-color', 'red');
                                $(".Liste_" + Id).hide(1000);

                            }
                            else {
                                ModalBox(data.Mesaj, "", "error", data.NotfyAlert, data.BootBoxAlert);
                            }
                        },
                        error: function (data, status, error) {
                            ModalBox(data.Mesaj, "", "error", data.NotfyAlert, data.BootBoxAlert);
                        }
                    });
                }
            }
        });




    }
    catch (err) {
        ModalBox(err.message, "", "error", true, false);
    }
}
