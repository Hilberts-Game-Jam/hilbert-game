"""
Convert raw monologue text files to a structured JSON format
Usage (while script file is in same directory as raw folder):
python raw_to_json.py
"""


import os
import json
import re

def process_raw_files():
    # Get the directory of this script
    script_dir = os.path.dirname(os.path.abspath(__file__))
    raw_dir = os.path.join(script_dir, "raw")
    output_file = os.path.join(script_dir, "monologues.json")
    
    # Dictionary to store the monologues by type
    monologues_dict = {}
    
    # Check if raw directory exists
    if not os.path.exists(raw_dir):
        print(f"Error: Raw directory not found at {raw_dir}")
        return
    
    # Process each .txt file in the raw directory
    for filename in os.listdir(raw_dir):
        if filename.endswith(".txt"):
            file_path = os.path.join(raw_dir, filename)
            
            try:
                with open(file_path, 'r', encoding='utf-8') as file:
                    content = file.read().strip()
                    
                    # Extract type from the first line
                    match = re.match(r'^type:\s*(.+)$', content.split('\n')[0])
                    if not match:
                        print(f"Warning: File {filename} does not have a valid type line. Skipping.")
                        continue
                    
                    monologue_type = match.group(1).strip()
                    
                    # Extract interactions (lines starting with '- ')
                    interactions = []
                    for line in content.split('\n')[1:]:
                        line = line.strip()
                        if line.startswith('- '):
                            interactions.append(line[2:].strip())
                    
                    # Add to the dictionary
                    if monologue_type not in monologues_dict:
                        monologues_dict[monologue_type] = []
                    
                    monologues_dict[monologue_type].extend(interactions)
                    
                    print(f"Processed {filename}: type={monologue_type}, interactions={len(interactions)}")
                    
            except Exception as e:
                print(f"Error processing {filename}: {str(e)}")
    
    # Write the result to a JSON file
    if monologues_dict:
        with open(output_file, 'w', encoding='utf-8') as json_file:
            json.dump(monologues_dict, json_file, indent=2, ensure_ascii=False)
        print(f"Successfully generated {output_file} with {len(monologues_dict)} monologue types")
    else:
        print("No valid monologues found to convert")

if __name__ == "__main__":
    process_raw_files()