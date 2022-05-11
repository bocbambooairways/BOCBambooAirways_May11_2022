$(function () {
    document.getElementById("incidentcode").disabled = true;
  
    $('#reporttype').multiSelect();
    $('#divisionrelated').multiSelect();
    $('#flightphase').multiSelect();

    $('.datepicker').click(
        function () {
            $('#txtDate').datepicker({
                format: "dd/mm/yyyy"
            });
        }
    );

    //Dùng để open fileUpload
    $('.custom-file-upload').bind('touchstart click', function () {
        $('#fileToUpload').click();
    });


     $('#fileToUpload').on('change', function () {
         const file = document.getElementById('fileToUpload').files[0];
         const size = file.size;
         // Check if the file size is bigger than 10MB and prepare a message.
         if (size > ((1024*1024)*10)) {
             alertify.alert("File Upload không được lớn hơn 10MB/Upload files no larger than 10MB");
             return false;
         } else {


             /*Append list file upload*/
             var formData = new FormData();
             var ins = document.getElementById('fileToUpload').files.length;
             for (var x = 0; x < ins; x++) {
                 formData.append("Files", document.getElementById('fileToUpload').files[x]);
             }
             ShowProgressAnimation();

             $.ajax({
                 type: "POST",
                 url: "/CR/ControlReport/UploadFileAttach",
                 data: formData,
                 /*use contentType, processData for sure.*/
                 contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
                 processData: false, // NEEDED, DON'T OMIT THIS

                 success: function (data) {
                     // Luu lai Session Object File Upload
                     sessionStorage.setItem('AttachedFile', JSON.stringify(data.obj));
                     json = JSON.parse(data.obj);
                     if (data.mess == "No such file") {
                          HideProgressAnimation();
                         alertify.alert("Đã có lỗi hệ thống không thể upload file. Vui lòng liên hệ admin.../There was an error that the system could not upload files. Please contact admin...");
                         return false;
                     }
                     if (data.mess = "OK") {
                         HideProgressAnimation();
                         /*parse json result ra string*/
                         var json = JSON.parse(data.obj);
                         var chuoi = "";
                         for (let i = 0; i < json.length; i++) {
                             var filepath = "/data/ControlReport/" + '/' + json[i].sysFileName;
                             if (json[i].Status == "OK") {
                                 obj = "<li>" + json[i].FileName + "&nbsp&nbsp-&nbsp&nbsp" + "<a  href=" + filepath + " ' target='_blank'" + " ' style='display: inline-block; width: 20px; height: 20px;color:blue;'" + "\" >View</a>" + "&nbsp&nbsp&nbsp&nbsp" + "<a class=\"removeFile\" href=\"#\"  style=\"display: inline - block; width: 20px; height: 20px; color: red;\" data-fileid=\"" + json[i].sysFileName + "\" >Remove</a>" + "</li> ";
                                 chuoi = chuoi + obj;
                             }
                         }

                         $('.fileList').html(chuoi);


                     }

                     else {
                         alertify.alert(data.mess);
                         return false;
                     }


                 },
                 error: function (data) {
                     alertify.alert(data.mess);
                     return false;
                 }
             })
         }

     });

    $(this).on("click", ".removeFile" ,function (e) {
        e.preventDefault();

        var formData = new FormData();
        var fileId = $(this).data("fileid");
        formData.append("t_fname", fileId);


        ShowProgressAnimation();
        $.ajax({
            type: "POST",
            url: "/CR/ControlReport/RemoveAttachFile",
            data: formData,
            //use contentType, processData for sure.
            contentType: false,
            processData: false,

            success: function (data) {

                if (data.mess = "OK") {
                    HideProgressAnimation();
                    // Luu lai Session Object File Upload
                    sessionStorage.setItem('AttachedFile', JSON.stringify(data.obj));
                    json = JSON.parse(data.obj);
                    var chuoi = "";
                    console.log(json);
                    for (let i = 0; i < json.length; i++) {
                        var filepath = "/data/ControlReport/" + '/' + json[i].sysFileName;
                        if (json[i].Status == "OK") {
                            var obj = "<li>" + json[i].FileName + "&nbsp&nbsp-&nbsp&nbsp" + "<a  href=" + filepath + " ' target='_blank'" + " ' style='display: inline-block; width: 20px; height: 20px;color:blue;'" + "\" >View</a>" + "&nbsp&nbsp&nbsp&nbsp" + "<a class=\"removeFile\" href=\"#\"  style=\"display: inline - block; width: 20px; height: 20px; color: red;\" data-fileid=\"" + json[i].sysFileName + "\" >Remove</a>" + "</li> ";
                            chuoi = chuoi + obj;
                        }

                    }
                    $('.fileList').html(chuoi);


                }
                else {
                    alertify.alert(data.mess);
                    return false;
                }


            },
            error: function (data) {
                alertify.alert(data.mess);
                return false;
            }
        })




    });


    $("#sendreport").click(function () {
        var formData = new FormData();

        var t_email = $("#email").val();
        if (t_email == "") {
            alertify.alert('Email không được bỏ trống/Email not be blank!');
            return false;
        }
        else {
            formData.append("Email", t_email);
        }

        var t_reportdate = $("#reportdate").val();
        if (t_reportdate == "") {
            alertify.alert('Ngày báo cáo không được bỏ trống/Report date not be blank!');
            return false;
        }
        else {
            formData.append("ReportDate", t_reportdate);
        }

        var t_reporttype = $("#reporttype").val();
        if (t_reporttype == "") {
            alertify.alert('Loại báo cáo không được bỏ trống/Report type not be blank!');
            return false;
        }
        else {


            formData.append("lst_ReportType_ID", t_reporttype);
        }

        var t_description = $("#description").val();
        if (t_description == "") {
            alertify.alert('Mô tả sự cố không được bỏ trống/Description of the incidient not be blank!');
            return false;
        }
        else {
            formData.append("Description", t_description);
        }


        var t_eventdate = $("#txtDate").val();
        if (t_eventdate == "") {
            alertify.alert('Ngày xảy ra sự cố không được bỏ trống/Date of incident not be blank!');
            return false;
        }
        else {
            formData.append("Event_Date", t_eventdate);
        }

        var t_eventlocation = $("#txtlocation").val();
        if (t_eventlocation == "") {
            alertify.alert('Nơi xảy ra sự cố không được bỏ trống/Location of incident not be blank!');
            return false;
        }
        else {
            formData.append("Event_Location", t_eventlocation);
        }



        var t_eventtime = $("#txtlocaltime").val();
        if (t_eventtime == "") {
            alertify.alert('Thời gian xảy ra sự cố không được bỏ trống/Local Time not be blank!');
            return false;
        }
        else {
            formData.append("Event_Time", t_eventtime);
        }

        var t_division = $("#divisionrelated").val();
        if (t_division == "") {
            alertify.alert('Bộ phận có liên quan đến sự cố không được bỏ trống/Division related to incident not be blank!');
            return false;
        }
        else {
            formData.append("lst_RpDivID", t_division);
        }

        var t_flightphase = $("#flightphase").val();
        if (t_flightphase == "") {
            alertify.alert('Giai đoạn bay không được bỏ trống/Flight phase not be blank!');
            return false;
        }
        else {
            formData.append("lst_FlightStage_ID", t_flightphase);
        }

        var t_reportcontent = $("#reportcontent").val();
        if (t_reportcontent == "") {
            alertify.alert('Nội dung báo cáo không được bỏ trống/Report Content not be blank!');
            return false;
        }
        else {
            formData.append("Content", t_reportcontent);
        }

        var t_recommendation = $("#recommendation").val();
        formData.append("Reccomendation", t_recommendation);

       
        var lst_attched = JSON.parse(sessionStorage.getItem('AttachedFile'));
        if (lst_attched == null) {
            alertify.alert('File đính kèm không được bỏ trống/Attach file not be blank!');
            return false;
        }

  
        ShowProgressAnimation();

        $.ajax({
            type: "POST",
            url: '@Url.Action("CreateReport", "ControlReport")',
            data: formData,
            processData: false,
            contentType: false,
            cache: false,
            success: function (data) {

                if (data.mess == "OK") {
                    HideProgressAnimation();
                    window.location.href = "/CR/ControlReport/ReportList";
                }

            },
            error: function (data) {
                alertify.alert(data.mess);
                return false;
            }

        });


    });
});


function ShowProgressAnimation() {

    document.getElementById("loading-div-background").style.zIndex = 9999;
    document.getElementById("loading-div-background").style.visibility = "visible";
    $("#loading-div-background").show();

}
function HideProgressAnimation() {
    document.getElementById("loading-div-background").style.visibility = "hidden";
    $("#loading-div-background").hide();

}





