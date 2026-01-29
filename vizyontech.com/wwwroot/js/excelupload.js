var loadingBar = (function () {

    $(document).ready(function () {

        $(document).on("click", ".btnEkle", function (e) {

            var btnActionValue = $(this).attr('btn-action');
      

            $("#final-result").css("display", "none");

            if (btnActionValue == "ExelImport") {
                var magaza = $("#MagazaId").val();
                if (magaza == 0) {
                    alert("Mağaza Seçiniz");
                }
                var fileExtension = ['xls', 'xlsx'];
                var filename = $('#fileupload').val();
                if (filename.length == 0) {
                    alert("Excel Dosyasını Seçiniz");
                    return false;
                }
                else {
                    var extension = filename.replace(/^.*\./, '');
                    if ($.inArray(extension, fileExtension) == -1) {
                        alert("Lütfen yalnızca excel dosyası seçin.");
                        return false;
                    }
                }


                var fdata = new FormData();

                var fileUpload = $("#fileupload").get(0);
                var files = fileUpload.files;
                fdata.append(files[0].name, files[0]);
                fdata.append('MagazaId', $('#MagazaId').val());
                fdata.append('UrunDurumu', $('#UrunDurumu').val());
                fdata.append('connectionId', $('#connectionId').val());


                $("#final-result").empty();

                let dataModel = {
                    fdata
                }

                let ajaxOptions = {
                    type: "POST",
                    data: fdata,
                    contentType: false,
                    processData: false,
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("XSRF-TOKEN",
                            $('input:hidden[name="__RequestVerificationToken"]').val());
                    },
                    url: "/Admin/" + btnActionValue,
                    success: function (result) {
                        $("#final-result").css("display", "block");
                        $("#final-result").removeClass("alert alert-danger").addClass("alert alert-" + result.MesajDurumu);
                        $("#final-result").html(result.Mesaj);
                        $("#fileupload").val(null);

                    },
                    error: function (result) {
                        $("#final-result").css("display", "block");
                        $("#final-result").removeClass("alert alert-success").addClass("alert alert-" + result.MesajDurumu);
                        $("#final-result").html(result.Mesaj);
                        $("#fileupload").val(null);
                    }
                };
            }
           
           
            let ajaxOptions = {
                type: "POST",
                url: "/Admin/" + btnActionValue,
                success: function (result) {
                    $("#final-result").css("display", "block");
                    $("#final-result").removeClass("alert alert-danger").addClass("alert alert-" + result.MesajDurumu);
                    $("#final-result").html(result.Mesaj);
                    $("#fileupload").val(null);

                },
                error: function (result) {
                    $("#final-result").css("display", "block");
                    $("#final-result").removeClass("alert alert-success").addClass("alert alert-" + result.MesajDurumu);
                    $("#final-result").html(result.Mesaj);
                    $("#fileupload").val(null);
                }
            };
            

            loadingWithProgressAndAbort.withSignalR(ajaxOptions);
        });
    });

})();


var loadingWithProgressAndAbort = (function () {

    function bindAbort(jqXHR) {

        $(document).on("click", "#loading-cancel", function () {
            jqXHR.abort();
            $("#final-result").html("Cancelled");
            enableLoadButton();
        });
    };

    function startLoading() {

        $("#div-loading").show();
    };

    function setProgress(progress) {

        $("#div-loading .progress-bar").attr("style", `width:${progress}%; transition:none;`);
        $("#div-loading .progress-bar").attr("aria-valuenow", progress);
        $("#div-loading .progress-bar").text(`${progress}% `);
    };

    function stopLoading() {

        $("#div-loading").hide();
        setProgress("#div-loading", 0);
    };

    function disableLoadButton() {

        let button = $(".btn-progress");
        button.prop("disabled", true);
        button.addClass("disable-hover");
    };

    function enableLoadButton() {

        let button = $(".btn-progress");
        button.prop("disabled", false);
        button.removeClass("disable-hover");

    };

    return {

        withSignalR: function (ajaxOptions) {



            disableLoadButton();

            var connection =
                new signalR.HubConnectionBuilder()
                    .withUrl("/loadingBarProgress")
                    .build();

            connection.on("updateLoadingBar",
                (perc) => {
                    var progress = Math.round((perc * 100));
                    setProgress(progress);
                    console.log(progress);
                });

            connection
                .start()
                .then(function () {

                    const connectionId = connection.connectionId;

                    if (ajaxOptions.data === undefined) {
                        ajaxOptions["data"] = {};
                    }
                    ajaxOptions.data["connectionId"] = connectionId;

                    const xhr = $.ajax(ajaxOptions);
                    bindAbort(xhr);
                    startLoading();

                    xhr.always(function () {
                        connection.stop();
                        stopLoading();
                        enableLoadButton();
                    });
                });
        }
    };
})();