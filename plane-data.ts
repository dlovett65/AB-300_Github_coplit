import { writeFileSync } from 'node:fs';
import path from 'node:path';

const csvData = `Id,Name,Year,Description,RangeInKm
1,Wright Flyer,1903,First powered airplane,39
2,Wright Flyer II,1904,Improved version of the Wright Flyer,56
3,Wright Glider,1900,Early glider experiment by the Wright brothers,18
4,Wright Model A,1903,First commercial aircraft design,97
5,Wright Model B,1905,Refined biplane for early flight demonstrations,120
6,Wright Military Flyer,1909,Military-oriented experimental aircraft,160
7,Kitty Hawk,1903,Name used for the Wright brothers' early flying experiments,45
8,Langley Aerodrome,1903,Government-funded experimental flying machine,24
9,Blériot XI,1909,Early monoplane that popularized aviation,120
10,Antoinette IV,1908,Elegant early aircraft used in pioneering flights,180`;

type Plane = {
  id: number;
  name: string;
  year: number;
  description: string;
  rangeInKm: number;
};

function parseCsvToJson(csv: string): Plane[] {
  const [headerLine, ...rows] = csv.trim().split(/\r?\n/);
  const headers = headerLine.split(',');

  return rows
    .filter(Boolean)
    .map((row) => {
      const values = row.split(',');
      const record = Object.fromEntries(
        headers.map((header, index) => [header.toLowerCase(), values[index]])
      ) as Record<string, string>;

      return {
        id: Number(record.id),
        name: record.name,
        year: Number(record.year),
        description: record.description,
        rangeInKm: Number(record.rangeinkm),
      };
    });
}

const planeData = parseCsvToJson(csvData);
const outputPath = path.join(process.cwd(), 'plane-data.json');

writeFileSync(outputPath, JSON.stringify(planeData, null, 2));

console.log(`Wrote ${planeData.length} plane records to ${outputPath}`);
