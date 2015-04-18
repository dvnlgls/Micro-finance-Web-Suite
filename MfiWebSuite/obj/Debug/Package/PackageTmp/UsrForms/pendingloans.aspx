<%@ Page Title="Pending Loans" Language="C#" MasterPageFile="~/MasterPages/default.Master" AutoEventWireup="true"
    CodeBehind="pendingloans.aspx.cs" Inherits="MfiWebSuite.UsrForms.pendingloans" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../DataTable/js/jquery.dataTables.min.js" type="text/javascript"></script>
    <style type="text/css">
        div.dataTables_length label
        {
            float: left;
            text-align: left;
        }
        
        div.dataTables_length select
        {
            width: 75px;
        }
        
        div.dataTables_filter label
        {
            float: right;
        }
        
        div.dataTables_info
        {
            padding-top: 8px;
            float: left;
        }
        
        div.dataTables_paginate
        {
            float: right;
            margin: 0;
        }
        
        table.table
        {
            clear: both;
            float: left;
            margin-bottom: 33px;
            margin-top: 30px;
        }
        
        table.table thead .sorting, table.table thead .sorting_asc, table.table thead .sorting_desc, table.table thead .sorting_asc_disabled, table.table thead .sorting_desc_disabled
        {
            cursor: pointer;
        }
        table.table thead .sorting
        {
            background: url('/DataTable/images/sort_both.png') no-repeat center right;
        }
        table.table thead .sorting_asc
        {
            background: url('/DataTable/images/sort_asc.png') no-repeat center right;
        }
        table.table thead .sorting_desc
        {
            background: url('/DataTable/images/sort_desc.png') no-repeat center right;
        }
        
        table.table thead .sorting_asc_disabled
        {
            background: url('/DataTable/images/sort_asc_disabled.png') no-repeat center right;
        }
        table.table thead .sorting_desc_disabled
        {
            background: url('/DataTable/images/sort_desc_disabled.png') no-repeat center right;
        }
    </style>
    <script type="text/javascript">
        /* Default class modification */
        $.extend($.fn.dataTableExt.oStdClasses, {
            "sWrapper": "dataTables_wrapper form-inline"
        });

        /* API method to get paging information */
        $.fn.dataTableExt.oApi.fnPagingInfo = function (oSettings) {
            return {
                "iStart": oSettings._iDisplayStart,
                "iEnd": oSettings.fnDisplayEnd(),
                "iLength": oSettings._iDisplayLength,
                "iTotal": oSettings.fnRecordsTotal(),
                "iFilteredTotal": oSettings.fnRecordsDisplay(),
                "iPage": Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength),
                "iTotalPages": Math.ceil(oSettings.fnRecordsDisplay() / oSettings._iDisplayLength)
            };
        }

        /* Bootstrap style pagination control */
        $.extend($.fn.dataTableExt.oPagination, {
            "bootstrap": {
                "fnInit": function (oSettings, nPaging, fnDraw) {
                    var oLang = oSettings.oLanguage.oPaginate;
                    var fnClickHandler = function (e) {
                        e.preventDefault();
                        if (oSettings.oApi._fnPageChange(oSettings, e.data.action)) {
                            fnDraw(oSettings);
                        }
                    };

                    $(nPaging).addClass('pagination').append(
				'<ul>' +
					'<li class="prev disabled"><a href="#">&larr; ' + oLang.sPrevious + '</a></li>' +
					'<li class="next disabled"><a href="#">' + oLang.sNext + ' &rarr; </a></li>' +
				'</ul>'
			);
                    var els = $('a', nPaging);
                    $(els[0]).bind('click.DT', { action: "previous" }, fnClickHandler);
                    $(els[1]).bind('click.DT', { action: "next" }, fnClickHandler);
                },

                "fnUpdate": function (oSettings, fnDraw) {
                    var iListLength = 5;
                    var oPaging = oSettings.oInstance.fnPagingInfo();
                    var an = oSettings.aanFeatures.p;
                    var i, j, sClass, iStart, iEnd, iHalf = Math.floor(iListLength / 2);

                    if (oPaging.iTotalPages < iListLength) {
                        iStart = 1;
                        iEnd = oPaging.iTotalPages;
                    }
                    else if (oPaging.iPage <= iHalf) {
                        iStart = 1;
                        iEnd = iListLength;
                    } else if (oPaging.iPage >= (oPaging.iTotalPages - iHalf)) {
                        iStart = oPaging.iTotalPages - iListLength + 1;
                        iEnd = oPaging.iTotalPages;
                    } else {
                        iStart = oPaging.iPage - iHalf + 1;
                        iEnd = iStart + iListLength - 1;
                    }

                    for (i = 0, iLen = an.length; i < iLen; i++) {
                        // Remove the middle elements
                        $('li:gt(0)', an[i]).filter(':not(:last)').remove();

                        // Add the new list items and their event handlers
                        for (j = iStart; j <= iEnd; j++) {
                            sClass = (j == oPaging.iPage + 1) ? 'class="active"' : '';
                            $('<li ' + sClass + '><a href="#">' + j + '</a></li>')
						.insertBefore($('li:last', an[i])[0])
						.bind('click', function (e) {
						    e.preventDefault();
						    oSettings._iDisplayStart = (parseInt($('a', this).text(), 10) - 1) * oPaging.iLength;
						    fnDraw(oSettings);
						});
                        }

                        // Add / remove disabled classes from the static elements
                        if (oPaging.iPage === 0) {
                            $('li:first', an[i]).addClass('disabled');
                        } else {
                            $('li:first', an[i]).removeClass('disabled');
                        }

                        if (oPaging.iPage === oPaging.iTotalPages - 1 || oPaging.iTotalPages === 0) {
                            $('li:last', an[i]).addClass('disabled');
                        } else {
                            $('li:last', an[i]).removeClass('disabled');
                        }
                    }
                }
            }
        });

        $(function () {
            if (jQuery("#ContentPlaceHolder1_PendingLoans").find("#tblLoans").length == 1) {

                $('#tblLoans').dataTable({
                    //"sDom": "<'row'<'span6'l><'span6'f>r>t<'row'<'span6'i><'span6'p>>",
                    "sPaginationType": "bootstrap",
                    "oLanguage": {
                        "sLengthMenu": "_MENU_ records per page"
                    },
                    "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }  ]
                });
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="gen-container-1">
        <div class="ho-content">
            <div class="gen-title">
                Loans Pending Disbursement &nbsp;<span class="badge badge-important lpen-count" id="LoanCount"
                    runat="server">0</span>
            </div>
            <div id="locNotHO" class="span12">
            </div>
            <div class="lpen-tbl" id="PendingLoans" runat="server">
            </div>
        </div>
    </div>
</asp:Content>
