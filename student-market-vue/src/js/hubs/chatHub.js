import * as signalR from "@microsoft/signalr";

const hubUrl = "https://localhost:9999/chat";
const connection = new signalR.HubConnectionBuilder()
    .withUrl(hubUrl)
    .build();
export default connection;