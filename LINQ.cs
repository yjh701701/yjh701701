return arrays.Length==0?new string[0]:arrays.Aggregate((a,b)=>a.Intersect(b).ToArray()).ToArray();

// Aggregate 배열 순차 순회 누적 연산시
// a.Intersect(b) a,b 두 컬렉션에서 교집합 반환 
