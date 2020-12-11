var dataTable;

$(document).ready(function () {
    console.log("doc ready");
    loadList();
})

function loadList() {
    console.log("Loading table!");
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/menuitem",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {"data": "name", "width": "25%"},
            {"data": "category.name", "width": "15%"},
            {"data": "foodType.name", "width": "15%"},
            {"data": "price", "width": "15%"},
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                             <a href="/Admin/MenuItem/upsert?id=${data}" class="btn btn-success" style="cursor: pointer; width:100px">
                             Edit
                             </a>
                             <a class="btn btn-danger" style="cursor: pointer; width:100px" onclick="Delete('/api/menuitem/'+'${data}')">
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