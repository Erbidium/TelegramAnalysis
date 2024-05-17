import { Component, OnInit } from '@angular/core';
import { EChartsOption } from "echarts";

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
    // @ts-ignore
    options: EChartsOption;

    createNodes(count: number) {
        const nodes = [];
        for (let i = 0; i < count; i++) {
            nodes.push({
                id: i + '',
                name: 'hello'
            });
        }
        return nodes;
    }

    getRandomInt(min: number, max: number) {
        // Use Math.floor to round down to the nearest whole number
        // Use Math.random() to generate a random decimal between 0 (inclusive) and 1 (exclusive)
        // Multiply by the range (max - min + 1) to cover the entire range
        // Add the minimum value to shift the range to [min, max]
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    createEdges(count: number) {
        const edges = [];
        if (count === 2) {
            return [[0, 1]];
        }
        for (let i = 0; i < count; i++) {
            edges.push([this.getRandomInt(0, 10), this.getRandomInt(0, 10)]);
        }
        return edges;
    }

    ngOnInit(): void {
        const datas = [];

        datas.push({
            nodes: this.createNodes(10),
            edges: this.createEdges(10),
        });

        this.options = {
            series: datas.map((item, idx) => {
                idx = 8;
                return {
                    type: 'graph',
                    layout: 'force',
                    ribbonType: true,
                    animation: true,
                    animationDuration: 1500,
                    animationEasingUpdate: 'quinticInOut',
                    emphasis: {
                        lineStyle: {
                            width: 10
                        }
                    },
                    data: item.nodes,
                    width: '100%',
                    height: '100%',
                    lineStyle: {
                        curveness: 0.3,
                        opacity: 0.9,
                        width: 2,
                    },
                    itemStyle: {
                        borderColor: '#fff',
                        borderWidth: 1,
                        shadowBlur: 10,
                        shadowColor: 'rgba(0, 0, 0, 0.3)'
                    },
                    symbolSize: 60,
                    draggable: true,
                    edgeSymbol: ['circle', 'arrow'],
                    roam: true,
                    focusNodeAdjacency: true,
                    force: {
                        repulsion: 4000,
                        edgeLength: 70,
                        gravity: 0.2
                    },
                    label: {
                        show: true,
                        position: "right"
                    },
                    labelLine: 'show',
                    edges: item.edges.map(e => {
                        return {
                            source: e[0] + '',
                            target: e[1] + ''
                        };
                    }),
                };
            }),
        };
    }
}
