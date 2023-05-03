const HConfig = {
    //url: "https://apidemo.laptrinhweb.edu.vn/api/v1/Employees/"
    URL: "https://localhost:9999/api/v1/",
    API: {
        Users: "https://localhost:9999/api/v1/Users/",
        Locations: "https://localhost:9999/api/v1/Locations/",
        Posts: "https://localhost:9999/api/v1/Posts/",
        Comments: "https://localhost:9999/api/v1/Comments/"
    },
    ListUserColumns: [
        {
            title: "Tài Khoản",
            dataField: "UserName",
            width: "160px",
            condition: {
                type: "contains",
                operator: "contains",
                conditionValue: null,
            },
        },
        {
            title: "Họ Tên",
            dataField: "FullName",
            width: "200px",
            condition: {
                type: "contains",
                operator: "contains",
                conditionValue: null,
            },
        },
        {
            title: "Giới Tính",
            formatType: "gender",
            dataField: "Gender",
            width: "140px",
            condition: {
                type: "select",
                operator: "equal",
                conditionValue: null,
            },
        },
        {
            title: "Ngày Sinh",
            dataField: "DateOfBirth",
            formatType: "date",
            width: "160px",
            dataAlign: "center",
            condition: {
                type: "date",
                operator: "smaller",
                conditionValue: null,
            },
        },
        {
            title: "Vai Trò",
            formatType: "role",
            dataField: "Role",
            width: "160px",
            condition: {
                type: "select",
                operator: "equal",
                conditionValue: null,
            },
        },
        {
            title: "Số CMND",
            dataField: "IdentityNumber",
            formatType: "text",
            width: "180px",
            note: "Số chứng minh nhân dân",
            condition: {
                type: "contains",
                operator: "contains",
                conditionValue: null,
            },
        },
        {
            title: "Email",
            dataField: "Email",
            width: "240px",
            condition: {
                type: "contains",
                operator: "contains",
                conditionValue: null,
            },
        },
        {
            title: "Khu Vực",
            dataField: "LocationName",
            width: "160px",
            condition: {
                type: "contains",
                operator: "contains",
                conditionValue: null,
            },
        },
        {
            title: "Địa chỉ",
            dataField: "Address",
            width: "240px",
            condition: {
                type: "contains",
                operator: "contains",
                conditionValue: null,
            },
        },
    ],
    ListPostColumns: [
        {
            title: "Mã Tin",
            dataField: "PostCode",
            width: "120px",
            condition: {
                type: "contains",
                operator: "contains",
                conditionValue: null,
            },
        },
        {
            title: "Tiêu Đề",
            dataField: "Title",
            width: "240px",
            condition: {
                type: "contains",
                operator: "contains",
                conditionValue: null,
            },
        },
        {
            title: "Người Đăng",
            dataField: "FullName",
            width: "240px",
            condition: {
                type: "contains",
                operator: "contains",
                conditionValue: null,
            },
        },
        {
            title: "Mô Tả",
            dataField: "PostDescribe",
            width: "320px",
            condition: {
                type: "contains",
                operator: "contains",
                conditionValue: null,
            },
        },
        {
            title: "Khu Vực",
            dataField: "LocationName",
            width: "160px",
            condition: {
                type: "contains",
                operator: "contains",
                conditionValue: null,
            },
        },
        {
            title: "Thời Gian",
            formatType: "createdDate",
            dataField: "CreatedDate",
            width: "220px",
            condition: {
                type: null,
                operator: null,
                conditionValue: null,
            },
        },
        {
            title: "Trạng Thái",
            formatType: "approved",
            dataField: "Approved",
            width: "140px",
            condition: {
                type: "select",
                operator: "equal",
                conditionValue: "0",
            },
        },
    ],
    ColumnWidthDefault: "120px"
};
export default HConfig;