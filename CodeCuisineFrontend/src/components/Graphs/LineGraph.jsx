import React, { useState } from "react";
import {
  Line,
  LineChart,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";

const LineGraph = (props) => {
  const [selectedStartDate, setSelectedStartDate] = useState("");
  const [selectedEndDate, setSelectedEndDate] = useState("");

  const filterData = () => {
    console.log("Selected Start Date:", selectedStartDate);
    console.log("Selected End Date:", selectedEndDate);

    if (!selectedStartDate || !selectedEndDate) {
      return props.chartData;
    }

    const startDateParts = selectedStartDate.split("/");
    const formattedStartDate = `${startDateParts[2]}-${startDateParts[1]}-${startDateParts[0]}`;
    const endDateParts = selectedEndDate.split("/");
    const formattedEndDate = `${endDateParts[2]}-${endDateParts[1]}-${endDateParts[0]}`;

    const startDate = new Date(formattedStartDate);
    const endDate = new Date(formattedEndDate);

    const filteredData = props.chartData.filter((entry) => {
      const entryParts = entry.day.split("/");
      const entryDate = new Date(
        `${entryParts[2]}-${entryParts[1]}-${entryParts[0]}`
      );
      console.log("Entry Date:", entryDate);
      return entryDate >= startDate && entryDate <= endDate;
    });

    console.log("Filtered Data:", filteredData);
    return filteredData;
  };

  const filteredData = filterData();

  return (
    <div className="w-full h-full">
      <div className="w-full h-1/3 flex flex-col items-center justify-center">
        <h1 className="text-2xl font-bold m-5 text-white">{props.title}</h1>
        <div className="flex w-1 text-xs items-center justify-center">
          <span className="text-white mx-2">De:</span>
          <div className="">
            <input
              type="date"
              value={selectedStartDate}
              onChange={(e) => setSelectedStartDate(e.target.value)}
              className="bg-white rounded-sm my-1"
              name="Date"
            />
          </div>
          <span className="text-white mx-2">Hasta:</span>
          <div className="">
            <input
              type="date"
              value={selectedEndDate}
              onChange={(e) => setSelectedEndDate(e.target.value)}
              className="bg-white rounded-sm my-1"
            />
          </div>
        </div>
      </div>

      <div className="charts w-full h-2/3 px-4">
        <ResponsiveContainer width="99%" height="100%">
          <LineChart
            data={filteredData}
            margin={{
              top: 5,
              right: 30,
              left: 20,
              bottom: 5,
            }}
          >
            <XAxis
              dataKey={props.dataKey}
              stroke="white"
              className="text-sm"
              padding={{ left: 30, right: 30 }}
            />
            <YAxis stroke="white" className="text-sm" />
            <Tooltip />
            <Line
              type="monotone"
              dataKey={props.dataValueKey}
              stroke={props.color}
              activeDot={{ r: 8 }}
              strokeWidth={2}
            />
          </LineChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
};

export default LineGraph;
