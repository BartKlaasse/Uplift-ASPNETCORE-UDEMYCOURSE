var dataTable;

$(document).ready(function() {
  loadDataTable();
});

function loadDataTable() {
  dataTable = $("#tblData").dataTable({
    ajax: {
      url: "/Admin/Category/GetAll",
      type: "GET",
      datatype: "json"
    },
    columns: [
      { data: "name", width: "50%" },
      { data: "displayOrder", width: "20%" },
      {
        data: "id",
        render: function(data) {
          return `<div class="text-center">
                                <a href="/Admin/Category/Upsert/${data}" class='btn btn-success text-white' style='cursor:pointer; width:100px;'>
                                <i class='far fa-edit'></i>Edit</a>
                                &nbsp;
                                <a onclick=Delete("/Admin/Category/Delete/${data}") class='btn btn-danger text-white' style='cursor:pointer; width:100px;'>
                                <i class='far fa-trash'></i>Delete</a>
                            </div>
                    
                    `;
        },
        width: "30%"
      }
    ],
    language: {
      emptyTable: "No Records found"
    },
    width: "100%"
  });
}
