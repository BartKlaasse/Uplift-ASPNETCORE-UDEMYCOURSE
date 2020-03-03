var dataTable;

$(document).ready(function() {
  loadDataTable();
});

function loadDataTable() {
  dataTable = $("#tblData").dataTable({
    ajax: {
      url: "/Admin/Frequency/GetAll",
      type: "GET",
      datatype: "json"
    },
    columns: [
      { data: "name", width: "50%" },
      { data: "frequencyCount", width: "20%" },
      {
        data: "id",
        render: function(data) {
          return `<div class="text-center">
                                <a href="/Admin/Frequency/Upsert/${data}" class='btn btn-success text-white' style='cursor:pointer; width:100px;'>
                                <i class='far fa-edit'></i>Edit</a>
                                &nbsp;
                                <a onclick=Delete("/Admin/Frequency/Delete/${data}") class='btn btn-danger text-white' style='cursor:pointer; width:100px;'>
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

function Delete(url) {
  swal(
    {
      title: "are you sure you want to delete this frequency?",
      text: "this action cannot be undone",
      type: "warning",
      showCancelButton: true,
      confirmButtonColor: "#DD6B55",
      confirmButtonText: "Delete",
      closeOnConfirm: true
    },
    function() {
      $.ajax({
        type: "DELETE",
        url: url,
        success: function(data) {
          if (data.success) {
            toastr.success(data.message);
            $("#tblData")
              .DataTable()
              .ajax.reload();
          } else {
            toastr.error(data.message);
          }
        }
      });
    }
  );
}
