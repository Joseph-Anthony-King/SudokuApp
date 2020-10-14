﻿import store from "../store";

export function requestHeader() {

    const token = store.getters["appConfigModule/getAuthToken"];

    if (token !== "") {

        return {
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*",
            Authorization: `Bearer ${token}`
        }

    } else {

        return {
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*"
        }
    }
}