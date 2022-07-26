import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import React from "react";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  }
];

export default AppRoutes;
