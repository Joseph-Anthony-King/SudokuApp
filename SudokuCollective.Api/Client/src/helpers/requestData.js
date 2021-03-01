﻿import store from "../store";

export function requestData(pageListModel, license = null) {

  if (license === null) {
    license = store.getters["settingsModule/getLicense"];
  }
  const requestorId = parseInt(
    store.getters["settingsModule/getRequestorId"]
  );
  const appId = parseInt(process.env.VUE_APP_ID);

  return {
    License: license,
    RequestorId: requestorId,
    AppId: appId,
    PageListModel: pageListModel,
  };
}
