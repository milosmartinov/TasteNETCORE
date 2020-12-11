var dataTable;

$(document).ready(function () {
    console.log("doc ready");
    loadList();
})

function loadList() {
    console.log("Loading table!");
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/category",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {"data": "name", "width": "40%"},
            {"data": "name", "width": "30%"},
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                             <a href="/Admin/category/upsert?id=${data}" class="btn btn-success" style="cursor: pointer; width:100px ">
                             Edit
                             </a>
                             <a class="btn btn-danger" style="cursor: pointer; width:100px " onclick="Delete('/api/category/'+'${data}')">
                             Delete
                             </a>
                             </div>`
                }, "width": "30%"
            },
        ],
        "language": {
            "emptyTable": "No table found!"
        },
        "width": "100%",

    })
}

function Delete(url) {
    swal(
        {
            title: "Are you sure?",
            message: "This action can not be undo!",
            icon: "warning",
            dangerMode: true,
            buttons: true
        }
    ).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.error) {
                        toastr.error(data.message);
                    } else {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                }
            })
        }
    })
}