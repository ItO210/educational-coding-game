import { PieChart, Pie, Cell, ResponsiveContainer, Tooltip } from "recharts";

const COLORS = ["#F6BA27", "#52BEDA"];

const PieGraph = (props) => {
  return (
    <div className="w-full h-full flex lg:flex-col">
      <div className="w-1/2 h-full lg:w-full lg:h-1/3 flex flex-col items-center justify-center">
        <h1 className="text-2xl font-bold my-2 text-white">{props.title}</h1>
      </div>

      <div className="charts w-1/2 h-full lg:h-2/3 lg:w-full px-0 lg:px-4">
        <ResponsiveContainer width="99%" height="100%">
          <PieChart>
            <Pie
              data={props.chartData}
              cx={"50%"}
              cy={"50%"}
              innerRadius={60}
              outerRadius={80}
              fill="#8884d8"
              paddingAngle={5}
              dataKey={props.dataKey}
              nameKey={props.dataValueKey}
            >
              {props.chartData.map((entry, index) => (
                <Cell
                  key={`cell-${index}`}
                  fill={COLORS[index % COLORS.length]}
                />
              ))}
            </Pie>
            <Tooltip />
          </PieChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
};

export default PieGraph;
