$(document).ready(function () {
    $('#sidebarCollapse').on('click', function () {
       $('#sidebar').toggleClass('active');
    });
});

function spinerLoading(spiner) {
    spiner.innerHTML = '<i style="margin-bottom:3px;" class="spinner-border spinner-border-sm"></i>';
}