function SaveEntry(form, validationFields) {
    debugger;
    var valid = true;
    validationFields.forEach(f => {
        if (form[f].value === "") {
            valid = false;
            form[f].classList.add("input-validation-error");
        }
    });
    if (!valid) return;
    $.ajax({
        url: form.action,
        type: form.method,
        data: $(form).serialize(),
        success: function (html) {
            $("#patients-id").prepend($(html));
            Reset(form, validationFields);
        },
        error: function (jqXHR, exception) {
            HandleError(jqXHR, exception);
        }
    });
}

function SaveResearchSuccess(html) {
    $("#researches-id").prepend($(html));
    HideResearchSection();
}

function SaveResearchError(jqXHR, exception) {
    HandleError(jqXHR, exception);
}

function RefreshPatientHistory(url, filterDoctor, page, skip) {
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify({ FilterDoctor: filterDoctor, Page: page, Skip: skip }),
        dataType: "html",
        success: function (msg) {
            if (skip === 0) {
                $("#patients-id").html(msg);
            }
            else {
                $("#patients-id").append($(msg));
                $("#direction").val(page);
            }
        },
        error: function (msg) { console.log(msg); }
    });
}
function RefreshResearchHistory(url, page, skip) {
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify({ Page: page, Skip: skip }),
        dataType: "html",
        success: function (msg) {
            if (skip === 0) {
                $("#researches-id").html(msg);
            }
            else {
                $("#researches-id").append($(msg));
                $("#page").val(page);
            }
        },
        error: function (msg) { console.log(msg); }
    });
}
function HandleError(jqXHR, exception) {
    var msg = "";
    if (jqXHR.status === 0) {
        msg = 'Not connect.\n Verify Network.';
    } else if (jqXHR.status == 404) {
        msg = 'Requested page not found. [404]';
    } else if (jqXHR.status == 500) {
        msg = 'Internal Server Error [500].';
    } else if (exception === 'parsererror') {
        msg = 'Requested JSON parse failed.';
    } else if (exception === 'timeout') {
        msg = 'Time out error.';
    } else if (exception === 'abort') {
        msg = 'Ajax request aborted.';
    } else {
        msg = 'Uncaught Error.\n' + jqXHR.responseText;
    }
    alert(msg);
}

function HideAddSection() {
    debugger;
    $("#add-section").hide();
    $("#addEntryLink").show();
}

function ShowAddSection() {
    debugger;
    $("#add-section").show();
    $("#addEntryLink").hide();
}


function ShowResearchSection() {
    debugger;
    $("#add-research").show();
    $("#addResearchLink").hide();
}

function HideResearchSection() {
    debugger;
    $("#add-research").hide();
    $("#addResearchLink").show();
}
function CurrentPage() {
    var value = $("#direction").val();
    return parseInt(value);
}
function IsFiltered() {
    var value = $("#filteredByDoctor").val();
    return value;
}


function Reset(form, validationFields) {
    debugger;
    form.reset();
    validationFields.forEach(f => form[f].classList.remove("input-validation-error"));
    HideAddSection();
}