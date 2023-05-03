import HCommon from './common.js';

const HResource = {
    MenuText: {
        CA: "Tiền mặt",
        Accounts: "Hệ thống tài khoản",
        Employees: "Nhân viên",
        CAProcess: "Quy trình",
        CAPayment: "Chi tiền",
    },

    Title: {
        FormAdd: "Thêm mới nhân viên",
        FormEdit: "Sửa thông tin nhân viên",
        CAProcess: "Nghiệp Vụ Tiền Mặt",
        CAReceipt: "Thu tiền",
        CAAudit: "Kiểm kê quỹ",
        Accounts: "Hệ thống tài khoản",
    },

    Text: {
        Accounts: {
            Searchbox: "Tìm kiếm theo số, tên tài khoản"
        },
        Search: "Tìm kiếm",
    },

    Message: {
        Exception: "Đã có lỗi xảy ra, xin vui lòng liên hệ với trung tâm hỗ trợ khách hàng",
        WarningMulteDelete: function (str) {
            str = (str < 10) ? `0${str}` : str;
            return `Bạn có chắc muốn xoá <b>${str}</b> nhân viên đã chọn không`
        },
        WarningDeleteRecord: function (str) {
            return `Bạn có chắc muốn xoá nhân viên <b>&lt;&lt;${str}&gt;&gt;</b> không`;
        },
        EmptyInput: " không được bỏ trống",
        DepartmentEmpty: "Đơn vị không được bỏ trống",
        FullNameEmpty: "Tên nhân viên không được bỏ trống",
        UserNameEmpty: "Mã nhân viên không được bỏ trống",
        DateOfBirthBiggerNow: "Ngày sinh không được lớn hơn hiện tại",
        IdentityDateBiggerNow: "Ngày cấp không được lớn hơn hiện tại",
        DataInvalid: function (str) {
            return `Dữ liệu ${str} không hợp lệ`;
        }
    },
    OptionsOperator: [
        { text: "Bằng", value: "equal" },
        { text: "Chứa", value: "contains" },
        { text: "Bắt đầu với", value: "startWith" },
        { text: "Kết thúc với", value: "endWith" },
        { text: "Nhỏ hơn", value: "smaller" },
        { text: "Nhỏ hơn hoặc bằng", value: "smallerOrEqual" },
        { text: "Lớn hơn", value: "bigger" },
        { text: "Lớn hơn hoặc bằng", value: "biggerOrEqual" },
        { text: "(Trống)", value: "isNull" },
        { text: "(Không Trống)", value: "isNotNull" },
    ],
    /**
     * Định dạng dữ liệu
     * Author: Nguyễn Văn Huy (05/03/2023)
     */
    formatData(field, data) {
        switch (field) {
            case "Gender":
                return HCommon.formatGender(data);
            case "Role":
                return HCommon.formatRole(data);
            case "Approved":
                return HCommon.formatApproved(data);
            default:
                return data;
        }
    },
    formatFilterText: function (condition) {
        var text = "";
        this.OptionsOperator.forEach((option) => {
            if (option.value == condition.Operator) {
                let value = (condition.Field == 'Gender') ? HCommon.formatGender(condition.Value) : condition.Value;
                value = (condition.Field == 'DateOfBirth') ? HCommon.formatDate(condition.Value, 1) : value;
                value = this.formatData(condition.Field, condition.Value);
                if (value) {
                    text = `${condition.Title} ${option.text} "${value}"`;
                } else {
                    text = `${condition.Title} ${option.text}`;
                }
            }
        });
        return text;
    },
    operatorText(operatorValue) {
        var text = "";
        this.OptionsOperator.forEach((option) => {
            if (option.value == operatorValue) {
                text = option.text;
            }
        });
        return text;
    },
};
export default HResource;