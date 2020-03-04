
$("#menu-toggle").click(function (e) {
    e.preventDefault();
    $("#wrapper").toggleClass("toggled");
});

$(document).ready(function () {
    var table = $('#mydatatable').DataTable({
        rowReorder: {
            selector: 'td:nth-child(2)'
        },
        responsive: true
    });
});
$(document).ready(function () {


    var i = 1;
    $('#assesment-add-button').click(function (e) {
        e.preventDefault();

        var title = document.getElementById("assesment-title").value;
        var marks = document.getElementById("assesment-marks").value;

        if (title != "" && marks != "") {
            i++;
            //var HTML = '<div id="row' + i + '"><div name="name[]" id="name" class="name_list">' + title + '</div><button id="' + i + '" name="remove" class="btn btn-danger btn_remove float-right"><i class="fa fa-trash"></i></button></div>'
            var HTML = '<tr id="row' + i + '"><td name="name[]" id="name" class="name_list">' + title + '</td><td>' + marks + '</td><td><button id="' + i + '" name="remove" class="btn btn-danger btn_remove float-right"><i class="fa fa-trash"></i></button></td></tr>'
            $('#dynamic_field').append(HTML);


            var hiddenTextArea = document.getElementById('hidden-text-box')

            var value = hiddenTextArea.value + title + "," + marks + "\n";
            hiddenTextArea.value = value;
        }
        else {
            alert("Field should not be empty");
        }
    })
    $(document).on('click', ".btn_remove", function () {

        var content = []
        var item = $(this).closest("tr").find('td').each(function (i, e) {

            content[i] = e.textContent;
        }) 
        console.log(content[0])
        var ass = content[0] + "," + content[1]

        var data = []
        var val = document.getElementById("hidden-text-box");
        data = val.value.split('\n');
        var res = "";
        for (var i = 0; i < data.length; i++) {
            if (data[i] != ass) {
                res += data[i] + '\n';
            }
        }
        val.value = res;


        var button_id = $(this).attr("id");
        $('#row' + button_id + '').remove();
    })

})

$(document).ready(function () {


    var i = 1;
    $('#field-add-button').click(function (e) {
        e.preventDefault();

        var title = document.getElementById("field-input").value;
        if (title != "") {
            i++;
            var HTML = '<tr id="row_2' + i + '"><td name="name[]" id="name" class="name_list">' + title + '</td><td><button id="' + i + '" name="remove" class="btn btn-danger btn_remove_2 float-right"><i class="fa fa-trash"></i></button></td></tr>'
            $('#dynamic_field_2').append(HTML);


            var hiddenTextArea = document.getElementById('hidden-text-box-2')

            var value = hiddenTextArea.value + title + ",";
            hiddenTextArea.value = value;
        } else {
            alert("Field should not be empty");
        }
    })
    $(document).on('click', ".btn_remove_2", function () {

        var content = []
        var item = $(this).closest("tr").find('td').each(function (i, e) {

            content[i] = e.textContent;
        })
        console.log("con = " + content[0]);

        var data = []
        var val = document.getElementById("hidden-text-box-2");
        data = val.value.split(',');
        var res = "";
        for (var i = 0; i < data.length; i++) {
            if (data[i] !="" && data[i] != content[0]) {
                res += data[i] + ',';
            }
        }
        val.value = res;

        var button_id = $(this).attr("id");
        $('#row_2' + button_id + '').remove();
    })

})

