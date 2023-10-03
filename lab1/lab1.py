import numpy as np
import sys
import math
import matplotlib.pyplot as plt

def generate_cities(cities_count):
    """Generate random x and y coordinates for cities."""
    x_coordinates = np.random.randint(100, size=cities_count)
    y_coordinates = np.random.randint(100, size=cities_count)
    return x_coordinates, y_coordinates

def generate_initial_population(population_size, cities_count):
    """Generate the initial population of possible routes."""
    cities = list(range(1, cities_count))
    population = []
    for _ in range(population_size):
        route = list(np.random.choice(cities, size=cities_count - 1, replace=False))
        route.insert(0, 0)
        population.append(route)
    return population

def euclidean_distance(x1, y1, x2, y2):
    """Calculate the Euclidean distance between two points."""
    return math.sqrt((x2 - x1) ** 2 + (y2 - y1) ** 2)

def select_best_routes(population, distances, population_size):
    """Select the best routes based on distances and population size."""
    selected_population = []
    for _ in range(population_size // 2):
        index = distances.index(min(distances))
        selected_population.append(population[index])
        distances.pop(index)
        population.pop(index)
    return selected_population

def calculate_total_distance(points, route):
    """Calculate the total distance of a route."""
    distance = 0
    for i in range(1, len(route)):
        distance += euclidean_distance(points[0][route[i - 1]], points[1][route[i - 1]], points[0][route[i]], points[1][route[i]])
    distance += euclidean_distance(points[0][route[-1]], points[1][route[-1]], points[0][route[0]], points[1][route[0]])
    return distance

def mutations(population, mutation_percentage):
    """Apply mutations to the population."""
    mutation_count = (len(population) * mutation_percentage) // 100
    for _ in range(mutation_count):
        mut_individual = np.random.randint(0, len(population) - 1)
        point_one = np.random.randint(1, len(population[0]) - 1)
        point_two = np.random.randint(1, len(population[0]) - 1)
        
        population[mut_individual][point_one], population[mut_individual][point_two] = population[mut_individual][point_two], population[mut_individual][point_one]
    
    return population

# panmixia
def choose_parents(population_size):
    """Choose pairs of parents for mating."""
    parents = []
    while len(parents) < population_size:
        parent1 = np.random.randint(0, population_size)
        parent2 = np.random.randint(0, population_size)
        if parent1 != parent2:
            parents.append((parent1, parent2))
    return parents

# outbreeding
def select_outbreeding_parents(population):
    """Select parents based on Hamming distance for outbreeding."""
    num_parents_to_select = len(population) * 2
    selected_indices = []
    
    while len(selected_indices) < num_parents_to_select:
        parent1_index = np.random.randint(0, len(population))
        parent1 = population[parent1_index]
        
        hamming_distances = [sum(gene1 != gene2 for gene1, gene2 in zip(parent1, other)) for other in population]
        
        parent2_index = np.argmax(hamming_distances)
        
        while parent2_index in selected_indices and parent2_index != 0:
            hamming_distances[parent2_index] = 0
            parent2_index = np.argmax(hamming_distances)

        selected_indices.append(parent1_index)
        selected_indices.append(parent2_index)
    
    return selected_indices

def crossover(individual1, individual2):
    """Perform crossover between two individuals."""
    count = len(individual1)
    point_one = np.random.randint(1, count - 3)
    point_two = np.random.randint(point_one + 1, count - 2)
    
    subpath1 = individual1[point_one:point_two]
    subpath2 = individual2[point_one:point_two]
    
    offspring1 = [gene for gene in individual1 if gene not in subpath2]
    offspring2 = [gene for gene in individual2 if gene not in subpath1]
    
    offspring1 = offspring1[:point_one] + subpath2 + offspring1[point_one:]
    offspring2 = offspring2[:point_one] + subpath1 + offspring2[point_one:]
    
    return offspring1, offspring2

def cyclic_crossover(parent1, parent2):
    """Perform cyclic crossover between two individuals."""
    count = len(parent1)
    cycle_start = np.random.randint(0, count)
    
    offspring1 = parent1.copy()
    offspring2 = parent2.copy()
    
    cycle = [False] * count
    cycle_index = cycle_start
    
    while True:
        cycle[cycle_index] = True
        value1 = parent1[cycle_index]
        value2 = parent2[cycle_index]
        
        offspring1[cycle_index] = value2
        offspring2[cycle_index] = value1
        
        cycle_index = parent1.index(value2) if cycle_index != parent1.index(value2) else parent2.index(value1)
        
        if cycle_index == cycle_start:
            break
    
    return offspring1, offspring2

def create_new_population(population, flag=False):
    """Create a new population through crossover."""
    new_population = []
    #parents = choose_parents(len(population))
    parents = select_outbreeding_parents(population)
    
    for i in range(0, len(parents) // 2 - 1, 2):
        if not flag:
            offspring1, offspring2 = cyclic_crossover(population[parents[i]], population[parents[i + 1]])
        else:
            offspring1, offspring2 = crossover(population[parents[i]], population[parents[i + 1]])
        new_population.extend([offspring1, offspring2])
    
    return new_population

def plot_route(cities_pos, route, color='black', alpha=0.75):
    """Plot a route on the city map."""
    x = [cities_pos[0][i] for i in route]
    y = [cities_pos[1][i] for i in route]
    x.append(x[0])
    y.append(y[0])
    plt.plot(x, y, color=color, alpha=alpha)


def main():
    print("Enter the number of iterations: ")
    iteration_count = int(input())
    print("Enter the number of cities: ")
    cities_count = int(input())
    print("Enter the mutation percentage: ")
    mutation_percentage = int(input())

    population_size = 100
    cities_pos = generate_cities(cities_count)
    
    print("Cities coordinates: ")
    for i in range(cities_count):
        print(f"City {i}: ({cities_pos[0][i]}, {cities_pos[1][i]})")

    population = generate_initial_population(population_size, cities_count)
    
    print("Initial population:")
    for i, individual in enumerate(population):
        print(f"Individual {i}: {individual}")

    fig, ax = plt.subplots(figsize=(6, 6))
    ax.scatter(cities_pos[0], cities_pos[1], c='black')

    distances = [calculate_total_distance(cities_pos, individual) for individual in population]
    print("Distance between cities for each individual:", distances)
    print("Minimum distance:", min(distances), '\n')
    best_route = min(distances)
    count = 1

    initial_population = population
    initial_distances = distances
    print("Cyclic crossover\n")
    temp = iteration_count
    while iteration_count > 0:
        count += 1
        iteration_count -= 1

        population = select_best_routes(population, distances, population_size)
        population = population + create_new_population(population)
        population = mutations(population, mutation_percentage)

        distances = [calculate_total_distance(cities_pos, individual) for individual in population]
        index = distances.index(max(distances))
        best_route = min(distances)
        print("Epoch: ", count, " best route length: ", best_route);
        if iteration_count != 0:
            plot_route(cities_pos, population[index], color=np.random.rand(3, ), alpha=0.75)
        else:
            x, y = [], []
            for i in range(cities_count):
                x.append(cities_pos[0][population[index][i]])
                y.append(cities_pos[1][population[index][i]])
            x.append(x[0])
            y.append(y[0])
            plt.plot(x, y, color='r', linewidth=5)

    outbreeding_result = best_route
    count = 1
    iteration_count = temp
    population = initial_population
    distances = initial_distances
    best_route = min(distances)
    print("Standard crossover\n")
    while iteration_count > 0:
        count += 1
        iteration_count -= 1

        population = select_best_routes(population, distances, population_size)
        population = population + create_new_population(population, True)
        population = mutations(population, mutation_percentage)

        distances = [calculate_total_distance(cities_pos, individual) for individual in population]
        index = distances.index(max(distances))
        best_route = min(distances)
        print("Epoch: ", count, " best route length: ", best_route);

    for i in range(cities_count):
        ax.annotate(str(i), (cities_pos[0][i] + 0.2, cities_pos[1][i] + 0.2), fontsize=16, fontweight='bold')
    plt.show()
    print ("Cyclic crossover result: ", outbreeding_result, " Standard crossover result: ", best_route)

if __name__ == "__main__":
	main()
