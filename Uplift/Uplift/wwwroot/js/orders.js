var dataTable;

$(document).ready(function() {
  var url = window.location.search;
  if (url.includes("approved")) {
    loadDataTable("GetAllApprovedOrders");
  } else {
    if (url.includes("all")) {
      loadDataTable("GetAllOrders");
    } else {
      loadDataTable("GetAllPendingOrders");
    }
  }
});

function loadDataTable(url) {
  dataTable = $("#tblData").dataTable({
    ajax: {
      url: "/Admin/Order/" + url,
      type: "GET",
      datatype: "json"
    },
    columns: [
      { data: "name", width: "20%" },
      { data: "PhoneNumber", width: "20%" },
      { data: "Email", width: "15%" },
      { data: "ServiceCount", width: "15%" },
      { data: "Status", width: "15%" },
      {
        data: "id",
        render: function(data) {
          return `<div class="text-center">
                                <a href="/Admin/Order/Details/${data}" class='btn btn-success text-white' style='cursor:pointer; width:100px;'>
                                <i class='far fa-edit'></i>Details</a>
                            </div>
                    `;
        },
        width: "15%"
      }
    ],
    language: {
      emptyTable: "No Records found"
    },
    width: "100%"
  });
}
